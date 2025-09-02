import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { Topbar } from "../../components/Topbar/Topbar";
import style from "./PainelAdm.module.css";
import { toast } from "react-toastify";
import { FaUserShield, FaUserGraduate } from "react-icons/fa";
import * as XLSX from "xlsx";
import { saveAs } from "file-saver";

// Array com estilos Dicebear para sorteio aleatório
const estilosDicebear = [
  "adventurer",
  "avataaars",
  "big-ears",
  "bottts",
  "croodles",
  "fun-emoji",
  "notionists",
  "shapes",
];

// Função que retorna um estilo Dicebear baseado no seed do usuário (ID ou nome)
function getEstiloAleatorio(seed) {
  let hash = 0;
  const str = seed.toString();
  for (let i = 0; i < str.length; i++) {
    hash = str.charCodeAt(i) + ((hash << 5) - hash);
  }
  const index = Math.abs(hash) % estilosDicebear.length;
  return estilosDicebear[index];
}

export function PainelAdm() {
  const navigate = useNavigate();

  const [busca, setBusca] = useState("");
  const [usuarios, setUsuarios] = useState([]);
  const [adminNome, setAdminNome] = useState("");
  const [adminImg, setAdminImg] = useState("");
  const [currentPage, setCurrentPage] = useState(1);
  const [usersPerPage] = useState(6);
  const [mostrarModal, setMostrarModal] = useState(false);
  const [usuarioParaRemover, setUsuarioParaRemover] = useState(null);
  const [isLoading, setIsLoading] = useState(true);

  useEffect(() => {
    const verificarAdmin = async () => {
      const id = localStorage.getItem("idUsuario");
      const token = localStorage.getItem("token");

      if (!id || !token) {
        navigate("/login");
        return;
      }

      try {
        const res = await fetch(
          `https://localhost:7076/Usuarios/ObterPorId/${id}`,
          {
            headers: { Authorization: `Bearer ${token}` },
          }
        );
        if (!res.ok) throw new Error("Erro ao carregar usuário");

        const data = await res.json();

        if (data.tipoUsuario !== 1) {
          navigate("/"); // não é admin
          return;
        }

        setAdminNome(data.nome);
        setAdminImg(data.imagemPerfilUrl || "/default-avatar.png");
      } catch (err) {
        console.error(err);
        navigate("/login");
      }
    };

    const carregarUsuarios = async () => {
      const token = localStorage.getItem("token");
      try {
        const res = await fetch("https://localhost:7076/Usuarios/ListarTodos", {
          headers: { Authorization: `Bearer ${token}` },
        });
        if (!res.ok) throw new Error("Erro ao carregar usuários");
        const data = await res.json();
        setUsuarios(data);
        setIsLoading(false);
      } catch (err) {
        console.error(err);
        toast.error("Erro ao carregar usuários");
        setIsLoading(false);
      }
    };

    const carregarTudo = async () => {
      await verificarAdmin();
      await carregarUsuarios();
    };

    carregarTudo();
  }, [navigate]);

  // Filtrar usuários por busca
  const usuariosFiltrados = usuarios.filter((u) => {
    const nome = u.nome?.toLowerCase() || "";
    const email = u.email?.toLowerCase() || "";
    const termo = busca.toLowerCase();

    return nome.includes(termo) || email.includes(termo);
  });

  // Exportar para Excel
  const exportarExcel = () => {
    const dados = usuarios.map((usuario) => ({
      Nome: usuario.nome,
      Email: usuario.email,
      Idioma: usuario.idiomaSelecionadoCodigo,
      XP: usuario.xp,
      Tipo: usuario.tipoUsuario === 1 ? "Admin" : "Aluno",
    }));
    const ws = XLSX.utils.json_to_sheet(dados);
    const wb = XLSX.utils.book_new();
    XLSX.utils.book_append_sheet(wb, ws, "Usuarios");
    const excelBuffer = XLSX.write(wb, { bookType: "xlsx", type: "array" });
    const blob = new Blob([excelBuffer], { type: "application/octet-stream" });
    saveAs(blob, `usuarios_${new Date().toISOString().slice(0, 10)}.xlsx`);
  };

  // Paginação
  const indexOfLastUser = currentPage * usersPerPage;
  const indexOfFirstUser = indexOfLastUser - usersPerPage;
  const currentUsers = usuariosFiltrados.slice(
    indexOfFirstUser,
    indexOfLastUser
  );
  const totalPages = Math.ceil(usuariosFiltrados.length / usersPerPage);

  const paginate = (pageNumber) => setCurrentPage(pageNumber);

  // Modal remover usuário
  const abrirModalRemover = (usuario) => {
    setUsuarioParaRemover(usuario);
    setMostrarModal(true);
  };

  const fecharModal = () => {
    setUsuarioParaRemover(null);
    setMostrarModal(false);
  };

  const removerUsuario = async () => {
    if (!usuarioParaRemover) return;
    const token = localStorage.getItem("token");
    try {
      const res = await fetch(
        `https://localhost:7076/Usuarios/Remover/${usuarioParaRemover.usuarioID}`,
        {
          method: "DELETE",
          headers: { Authorization: `Bearer ${token}` },
        }
      );
      if (!res.ok) throw new Error("Erro ao remover usuário");

      setUsuarios((prev) =>
        prev.filter((u) => u.usuarioID !== usuarioParaRemover.usuarioID)
      );
      toast.success("Usuário removido com sucesso");
      fecharModal();
    } catch (err) {
      console.error(err);
      toast.error("Erro ao remover usuário");
    }
  };

  if (isLoading) {
    return <div className={style.loading}>Carregando painel...</div>;
  }

  return (
    <div className={style.container}>
      <Topbar />
      
      <main className={style.mainContent}>
        <header className={style.header}>
          <img src={adminImg} alt="Admin" className={style.avatar} />
          <div className={style.headerText}>
            <h2>Painel do Administrador</h2>
            <p>Bem-vindo(a), {adminNome}</p>
          </div>
          <button onClick={exportarExcel} className={style.exportButton}>
            Exportar Excel
          </button>
        </header>

        <input
          type="text"
          placeholder="Buscar por nome ou e-mail..."
          value={busca}
          onChange={(e) => setBusca(e.target.value)}
          className={style.searchInput}
        />

        <section className={style.userGrid}>
          {currentUsers.map((usuario) => (
            <div key={usuario.usuarioID} className={style.userCard}>
              <div className={style.userHeader}>
                <img
                  src={
                    usuario.imagemPerfilUrl?.trim()
                      ? usuario.imagemPerfilUrl
                      : `https://api.dicebear.com/7.x/${getEstiloAleatorio(
                          usuario.usuarioID || usuario.nome
                        )}/svg?seed=${encodeURIComponent(
                          usuario.nome || usuario.usuarioID
                        )}`
                  }
                  alt={usuario.nome}
                  className={style.userAvatar}
                  onError={(e) => {
                    e.target.onerror = null;
                    e.target.src = "/default-avatar.png";
                  }}
                />

                <div className={style.userInfo}>
                  <h4>{usuario.nome}</h4>
                  <span
                    className={`${style.userType} ${
                      usuario.tipoUsuario === 1
                        ? style.adminBadge
                        : style.studentBadge
                    }`}
                  >
                    {usuario.tipoUsuario === 1 ? (
                      <>
                        <FaUserShield /> Admin
                      </>
                    ) : (
                      <>
                        <FaUserGraduate /> Aluno
                      </>
                    )}
                  </span>
                </div>
              </div>
              <div className={style.userDetails}>
                <div className={style.detailRow}>
                  <span className={style.detailLabel}>Email:</span>
                  <span className={style.detailValue}>{usuario.email}</span>
                </div>
                <div className={style.detailRow}>
                  <span className={style.detailLabel}>Idioma:</span>
                  <span className={style.detailValue}>
                    {usuario.idiomaSelecionadoCodigo}
                  </span>
                </div>
                <div className={style.detailRow}>
                  <span className={style.detailLabel}>XP:</span>
                  <span className={style.detailValue}>{usuario.xp}</span>
                </div>
              </div>
              <div className={style.cardActions}>
                <button
                  className={style.btnDanger}
                  onClick={() => abrirModalRemover(usuario)}
                >
                  Remover
                </button>
              </div>
            </div>
          ))}
        </section>

        {usuariosFiltrados.length > usersPerPage && (
          <div className={style.pagination}>
            {Array.from({ length: totalPages }, (_, i) => i + 1).map((n) => (
              <button
                key={n}
                onClick={() => paginate(n)}
                className={`${style.pageButton} ${
                  currentPage === n ? style.active : ""
                }`}
              >
                {n}
              </button>
            ))}
          </div>
        )}

        {mostrarModal && (
          <div className={style.modalOverlay}>
            <div className={style.modal}>
              <h3>Confirmar remoção</h3>
              <p>
                Deseja remover <b>{usuarioParaRemover.nome}</b>?
              </p>
              <div className={style.modalActions}>
                <button className={style.btn} onClick={removerUsuario}>
                  Sim
                </button>
                <button className={style.btnDanger} onClick={fecharModal}>
                  Não
                </button>
              </div>
            </div>
          </div>
        )}
      </main>
    </div>
  );
}
