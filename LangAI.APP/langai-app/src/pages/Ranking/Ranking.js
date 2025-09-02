import { useEffect, useState } from "react";
import axios from "axios";
import style from "./Ranking.module.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import avatarDefault from "../../assets/avatar-default.png";
import PageTitle from "../../components/PageTitle/PageTitle";

export default function Ranking() {
  const [dados, setDados] = useState([]);
  const [paginaAtual, setPaginaAtual] = useState(1);
  const itensPorPagina = 8; // Alterado de 15 para 8

  const fetchRanking = async () => {
    try {
      const token = localStorage.getItem("token");
      if (!token) return;

      const response = await axios.get(
        "https://localhost:7076/Usuarios/ListarTodos",
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      const estilos = [
        "fun-emoji",
        "adventurer",
        "pixel-art",
        "bottts",
        "lorelei",
        "thumbs",
      ];

      const dadosComFotos = response.data.map((aluno) => {
        const temFoto = aluno.imagemPerfilUrl?.trim();

        const estiloAleatorio =
          estilos[Math.floor(Math.random() * estilos.length)];
        const seed = aluno.nome || aluno.usuarioID;

        const fotoUrl = temFoto
          ? aluno.imagemPerfilUrl
          : `https://api.dicebear.com/7.x/${estiloAleatorio}/svg?seed=${encodeURIComponent(
              seed
            )}`;

        return { ...aluno, foto: fotoUrl };
      });

      setDados(dadosComFotos);
    } catch (error) {
      console.error("Erro ao carregar ranking:", error);
    }
  };

  useEffect(() => {
    fetchRanking();
  }, []);

  const alunosOrdenados = [...dados].sort((a, b) => (b.xp || 0) - (a.xp || 0));
  const indexUltimoItem = paginaAtual * itensPorPagina;
  const indexPrimeiroItem = indexUltimoItem - itensPorPagina;
  const alunosPaginaAtual = alunosOrdenados.slice(
    indexPrimeiroItem,
    indexUltimoItem
  );

  const paginar = (numeroPagina) => setPaginaAtual(numeroPagina);

  return (
    <Sidebar>
      <Topbar>
        <div className={style.container}>
          <PageTitle>Ranking</PageTitle>
          <div className={style.mainContent}>
            <div className={style.listaRanking}>
              {alunosPaginaAtual.map((aluno, index) => (
                <div key={aluno.usuarioID} className={style.itemRanking}>
                  <div className={style.posicao}>
                    {indexPrimeiroItem + index + 1}º
                  </div>
                  <div className={style.infoAluno}>
                    <img
                      src={aluno.foto}
                      alt={aluno.nome}
                      className={style.fotoAluno}
                      onError={(e) => {
                        e.target.onerror = null;
                        e.target.src = avatarDefault;
                      }}
                    />

                    <div className={style.detalhesAluno}>
                      <div className={style.nomeAluno}>{aluno.nome}</div>
                      <div className={style.pontuacao}>{aluno.xp ?? 0} XP</div>
                    </div>
                  </div>
                  <div className={style.nivelAluno}>
                    Nível {Math.floor((aluno.xp || 0) / 100 + 1)}
                  </div>
                </div>
              ))}
            </div>

            {dados.length > itensPorPagina && (
              <div className={style.paginacaoContainer}>
                <div className={style.paginacao}>
                  {Array.from({
                    length: Math.ceil(alunosOrdenados.length / itensPorPagina),
                  }).map((_, index) => (
                    <button
                      key={index}
                      onClick={() => paginar(index + 1)}
                      className={`${style.botaoPagina} ${
                        paginaAtual === index + 1 ? style.ativo : ""
                      }`}
                    >
                      {index + 1}
                    </button>
                  ))}
                </div>
              </div>
            )}
          </div>
        </div>
      </Topbar>
    </Sidebar>
  );
}
