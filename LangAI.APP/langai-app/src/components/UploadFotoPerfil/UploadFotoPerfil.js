import { useState, useRef } from "react";
import { FaCamera } from "react-icons/fa";
import style from "./UploadFotoPerfil.module.css";

export function UploadFotoPerfil({ onUpload }) {
  const [isUploading, setIsUploading] = useState(false);
  const inputRef = useRef();

  const handleUpload = async (e) => {
    const file = e.target.files[0];
    if (!file) return;

    setIsUploading(true);

    try {
      if (file.size > 5 * 1024 * 1024) {
        throw new Error("A imagem deve ter no máximo 5MB");
      }

      const token = localStorage.getItem("token");
      const userId = localStorage.getItem("idUsuario");

      if (!token || !userId) throw new Error("Usuário não autenticado");

      const extensao = file.name.split(".").pop().toLowerCase(); // jpg, png...

      const presignedResp = await fetch(
        `https://localhost:7076/Usuarios/gerar-url-upload?usuarioId=${userId}&extensao=${extensao}`,
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (!presignedResp.ok) throw new Error("Erro ao gerar URL de upload");

      const { uploadUrl, fileUrl } = await presignedResp.json();

      const s3Resp = await fetch(uploadUrl, {
        method: "PUT",
        body: file,
        headers: {
          "Content-Type": file.type,
        },
      });

      if (!s3Resp.ok)
        throw new Error(`Erro ao enviar para S3: ${s3Resp.status}`);

      const atualizarResp = await fetch(
        "https://localhost:7076/Usuarios/atualizar-foto",
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            UsuarioID: parseInt(userId),
            imagemPerfilUrl: fileUrl,
          }),
        }
      );

      if (!atualizarResp.ok)
        throw new Error("Erro ao atualizar URL no banco");

      onUpload(fileUrl + "?t=" + Date.now());
    } catch (error) {
      console.error("Erro no upload:", error);
      alert(error.message || "Erro desconhecido");
    } finally {
      setIsUploading(false);
    }
  };

  return (
    <div className={style.container}>
      <input
        type="file"
        accept="image/*"
        className={style.inputHidden}
        onChange={handleUpload}
        ref={inputRef}
        disabled={isUploading}
      />

      <button
        type="button"
        className={style.botaoCustomizado}
        onClick={() => inputRef.current.click()}
        disabled={isUploading}
      >
        <FaCamera className={style.icon} />
        {isUploading ? "Enviando..." : "Alterar Foto"}
      </button>
    </div>
  );
}
