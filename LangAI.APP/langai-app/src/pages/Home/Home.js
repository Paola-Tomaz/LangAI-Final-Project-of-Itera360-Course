import React, { useEffect, useState } from "react";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import { WelcomeCard } from "../../components/WelcomeCard/WelcomeCard";
import { PracticeCard } from "../../components/PracticeCard/PracticeCard";
import style from "./Home.module.css";

export function Home() {
  const [nome, setNome] = useState("");
  const [imgUrl, setImgUrl] = useState("");

  useEffect(() => {
    const carregarUsuario = async () => {
      const id = localStorage.getItem("idUsuario");
      const token = localStorage.getItem("token");

      try {
        const response = await fetch(`https://localhost:7076/Usuarios/ObterPorId/${id}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (!response.ok) throw new Error("Erro ao buscar dados do usuário");

        const data = await response.json();
        setNome(data.nome);
        setImgUrl(`${data.imagemPerfilUrl}?t=${Date.now()}`);
      } catch (error) {
        console.error("Erro ao carregar usuário:", error);
      }
    };

    carregarUsuario();
  }, []);

  return (
    <div className={style.content}>
      <Sidebar>
        <Topbar>
          <div className={style.page_content}>
            <WelcomeCard name={nome} imgUrl={imgUrl} />

            <h2 className={style.title}>O que você quer praticar hoje?</h2>
            <div className={style.practice_cards}>
              <PracticeCard
                className={style.first_card}
                label="Listening"
                iconUrl="https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/cards/listening.png"
              />
              <PracticeCard
                className={style.first_card}
                label="Reading"
                iconUrl="https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/cards/reading.png"
              />
              <PracticeCard
                className={style.first_card}
                label="Speaking"
                iconUrl="https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/cards/speaking.png"
              />
            </div>
          </div>
        </Topbar>
      </Sidebar>
    </div>
  );
}
