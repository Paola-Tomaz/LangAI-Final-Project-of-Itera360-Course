import React from "react";
import style from "./WelcomeCard.module.css";

export function WelcomeCard({ name, imgUrl }) {
  return (
    <div className={style.card}>
      <div className={style.text}>
        <h2>
          Bem vindo, <strong>{name}!</strong>
        </h2>
        <div className={style.paragrafo}>
          <p>
            Sua jornada no aprendizado de idiomas começa aqui.
            <br />
            <span>Conte com a IA para evoluir um pouco mais a cada dia!</span>
          </p>
        </div>
      </div>
      <div className={style.avatarContainer}>
        <img
          src={imgUrl || "/assets/img/default-avatar.png"}
          className={style.avatar}
        />
      </div>
    </div>
  );
}
