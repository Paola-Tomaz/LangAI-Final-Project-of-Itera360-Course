import React from "react";
import styles from "./Idioma.module.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import { useNavigate } from "react-router-dom";
import PageTitle from "../../components/PageTitle/PageTitle";

const idiomas = [
  {
    nome: "Klaus",
    idioma: "alemão",
    cor: "#255736",
    botao: "Começar",
    imagem: "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/personagens/klaus.png",
  },
  {
    nome: "Lola",
    idioma: "inglês",
    cor: "#EEB04D",
    botao: "Começar",
    imagem: "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/personagens/lola.png",
  },
  {
    nome: "Sebastian",
    idioma: "francês",
    cor: "#255736",
    botao: "Começar",
    imagem: "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/personagens/sebastian.png",
  },
  {
    nome: "Miyuki",
    idioma: "japonês",
    cor: "#9C27B0",
    botao: "Começar",
    imagem: "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/personagens/miyuki.png",
  },
];

export default function Idioma() {
  const navigate = useNavigate();
  
  return (
    <Sidebar>
      <Topbar>
        <div className={styles.container}>
          <PageTitle>Idioma</PageTitle>
          <div className={styles.contentWrapper}>
            <div className={styles.gridContainer}>
              {idiomas.map((item, index) => (
                <div key={index} className={styles.card}>
                  <div className={styles.avatarContainer}>
                    <img
                      src={item.imagem}
                      alt={item.nome}
                      className={styles.avatar}
                    />
                  </div>
                  <div className={styles.cardBody} style={{ borderColor: item.cor }}>
                    <div className={styles.textContent}>
                      <p className={styles.cardText}>
                        Sou {item.nome === "Lola" ? "a" : "o"} {item.nome} e vou
                        te ensinar {item.idioma}!
                      </p>
                      <button
                        className={styles.cardButton}
                        style={{ backgroundColor: item.cor }}
                        onClick={() => {
                          localStorage.setItem("idiomaSelecionado", item.idioma);
                          localStorage.setItem(
                            "personagemSelecionado",
                            JSON.stringify(item)
                          );
                          navigate("/exercicio");
                        }}
                      >
                        {item.botao}
                      </button>
                    </div>
                  </div>
                </div>
              ))}
            </div>
          </div>
        </div>
      </Topbar>
    </Sidebar>
  );
}