import React, { useState, useEffect } from "react";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import { UploadFotoPerfil } from "../../components/UploadFotoPerfil/UploadFotoPerfil";
import styles from "./Perfil.module.css";

const Perfil = () => {
  const [foto, setFoto] = useState(null);
  const [nome, setNome] = useState("");
  const [email, setEmail] = useState("");
  const [telefone, setTelefone] = useState("");
  const [endereco, setEndereco] = useState("");
  const [descricao, setDescricao] = useState("");
  const [senha, setSenha] = useState("");
  const [confSenha, setConfSenha] = useState("");
  const [mensagem, setMensagem] = useState("");

  useEffect(() => {
    async function carregarPerfil() {
      const idUsuario = localStorage.getItem("idUsuario");
      const token = localStorage.getItem("token");

      if (!idUsuario || !token) {
        console.warn("Usuário ou token não encontrados.");
        return;
      }

      try {
        const response = await fetch(
          `https://localhost:7076/Usuarios/ObterPorId/${idUsuario}`,
          {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          }
        );

        if (!response.ok) {
          throw new Error(`Erro HTTP: ${response.status}`);
        }

        const data = await response.json();
        setNome(data.nome);
        setEmail(data.email);
        setFoto(data.imagemPerfilUrl || null);
        setTelefone(data.telefone || "");
        setEndereco(data.endereco || "");
        setDescricao(data.descricao || "");
      } catch (error) {
        console.error("Erro ao carregar perfil:", error);
      }
    }

    carregarPerfil();
  }, []);

  async function handleSalvar(e) {
    e.preventDefault();

    const token = localStorage.getItem("token");
    const idUsuario = localStorage.getItem("idUsuario");

    if (!token || !idUsuario) {
      setMensagem("Token ou usuário não encontrado.");
      return;
    }

    if (senha && senha !== confSenha) {
      setMensagem("As senhas não coincidem!");
      return;
    }

    try {
      // Atualizar dados de perfil
      const perfilResponse = await fetch(
        "https://localhost:7076/Usuarios/atualizar",
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${token}`,
          },
          body: JSON.stringify({
            usuarioID: parseInt(idUsuario),
            nome,
            email,
            telefone,
            endereco,
            descricao,
          }),
        }
      );

      if (!perfilResponse.ok) {
        throw new Error("Erro ao atualizar dados de perfil.");
      }

      // Atualizar senha (se fornecida)
      if (senha) {
        const senhaResponse = await fetch(
          "https://localhost:7076/Usuarios/alterar-senha",
          {
            method: "PUT",
            headers: {
              "Content-Type": "application/json",
              Authorization: `Bearer ${token}`,
            },
            body: JSON.stringify({
              usuarioID: parseInt(idUsuario),
              novaSenha: senha,
            }),
          }
        );

        if (!senhaResponse.ok) {
          throw new Error("Erro ao atualizar a senha.");
        }
      }

      setMensagem("Perfil atualizado com sucesso!");
      setSenha("");
      setConfSenha("");

      // Atualiza localStorage
      const userData = JSON.parse(localStorage.getItem("userData") || "{}");
      userData.nome = nome;
      userData.email = email;
      localStorage.setItem("userData", JSON.stringify(userData));
    } catch (err) {
      console.error(err);
      setMensagem("Erro ao atualizar perfil. Tente novamente.");
    }
  }

  const handleUploadConcluido = async (url) => {
    const tokenAtual = localStorage.getItem("token");
    const userId = localStorage.getItem("idUsuario");

    if (!tokenAtual || !userId) {
      setMensagem("Você precisa estar logado para alterar a foto.");
      return;
    }

    try {
      const response = await fetch(
        "https://localhost:7076/Usuarios/atualizar-foto",
        {
          method: "PUT",
          headers: {
            "Content-Type": "application/json",
            Authorization: `Bearer ${tokenAtual}`,
          },
          body: JSON.stringify({
            UsuarioID: parseInt(userId),
            imagemPerfilUrl: url,
          }),
        }
      );

      if (!response.ok) {
        const errorData = await response.json();
        throw new Error(errorData.message || "Erro ao atualizar foto");
      }

      setFoto(url);
      setMensagem("Foto de perfil atualizada com sucesso!");

      const userData = JSON.parse(localStorage.getItem("userData") || "{}");
      userData.imagemPerfilUrl = url;
      localStorage.setItem("userData", JSON.stringify(userData));
    } catch (error) {
      console.error("Erro ao salvar imagem:", error);
      setMensagem("Erro ao salvar imagem. Tente novamente.");
    }
  };

  return (
    <Sidebar>
      <Topbar>
        <div className={styles.containerPerfil}>
          <div className={styles.boxFoto}>
            <div>
              {foto ? (
                <img
                  src={`${foto}?${Date.now()}`}
                  alt="Foto de perfil"
                  className={styles.fotoPreview}
                  loading="lazy"
                  decoding="async"
                  onError={(e) => {
                    e.target.src = "/default-avatar.png";
                    e.target.onerror = null;
                  }}
                />
              ) : (
                <div className={styles.avatarPlaceholder}>
                  {nome ? nome.charAt(0).toUpperCase() : "?"}
                </div>
              )}
            </div>
            <UploadFotoPerfil onUpload={handleUploadConcluido} />
            <textarea
              placeholder="Descrição / Biografia"
              value={descricao}
              onChange={(e) => setDescricao(e.target.value)}
              className={styles.textareaDescricao}
            />
          </div>

          <form onSubmit={handleSalvar} className={styles.formularioPerfil}>
            <h2>Editar Perfil</h2>

            <label className={styles.labelBloco}>
              Nome Completo
              <input
                type="text"
                value={nome}
                onChange={(e) => setNome(e.target.value)}
                required
                className={styles.inputEstilizado}
              />
            </label>

            <div className={styles.inputGrupo}>
              <label className={styles.inputFlex}>
                E-mail
                <input
                  type="email"
                  value={email}
                  onChange={(e) => setEmail(e.target.value)}
                  required
                  className={styles.inputEstilizado}
                />
              </label>
              <label className={styles.inputFlex}>
                Telefone
                <input
                  type="tel"
                  value={telefone}
                  onChange={(e) => setTelefone(e.target.value)}
                  className={styles.inputEstilizado}
                />
              </label>
            </div>

            <label className={styles.labelBloco}>
              Endereço
              <input
                type="text"
                value={endereco}
                onChange={(e) => setEndereco(e.target.value)}
                className={styles.inputEstilizado}
              />
            </label>

            <div className={styles.inputGrupo}>
              <label className={styles.inputFlex}>
                Nova Senha
                <input
                  type="password"
                  value={senha}
                  onChange={(e) => setSenha(e.target.value)}
                  className={styles.inputEstilizado}
                />
              </label>
              <label className={styles.inputFlex}>
                Confirmar Senha
                <input
                  type="password"
                  value={confSenha}
                  onChange={(e) => setConfSenha(e.target.value)}
                  className={styles.inputEstilizado}
                />
              </label>
            </div>

            <button type="submit" className={styles.botaoSalvar}>
              Salvar
            </button>

            {mensagem && (
              <p
                className={styles.mensagem}
                style={{
                  color: mensagem.includes("sucesso") ? "green" : "red",
                }}
              >
                {mensagem}
              </p>
            )}
          </form>
        </div>
      </Topbar>
    </Sidebar>
  );
};

export default Perfil;
