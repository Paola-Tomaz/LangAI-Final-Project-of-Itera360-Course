import React, { useEffect, useState } from "react";
import style from "./Sidebar.module.css";
import { SidebarItem } from "../SidebarItem/SidebarItem";
import { motion } from "framer-motion";
import {
  FaBars,
  FaHome,
  FaUser,
  FaLanguage,
  FaArrowUp,
  FaBarcode,
} from "react-icons/fa";
import { GrNotes } from "react-icons/gr";

export function Sidebar({ children }) {
  const [aberto, setAberto] = useState(true);

  useEffect(() => {
    if (window.innerWidth <= 768) {
      setAberto(false);
    }
  }, [])

  return (
    <div className={style.sidebarWrapper}>
      <motion.div
        animate={{ width: aberto ? 250 : 60 }}
        transition={{ duration: 0.3, type: "spring", damping: 15 }}
        className={style.sidebar_content}
      >
        <div className={style.sidebar_header}>
          
          <button className={style.hamburguer} onClick={() => setAberto(!aberto)}>
            <FaBars />
          </button>
          
          {aberto && (
            <img
              src="https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/Logo/Logo2.png"
              alt="Logo LangAI"
              className={style.logo}
            />
          )}
          <hr className={style.line} />
        </div>

        <div className={style.sidebar_body}>
          <SidebarItem text="Home" link="/home" logo={<FaHome />} mostrarTexto={aberto} />
          <SidebarItem text="Perfil" link="/perfil" logo={<FaUser />} mostrarTexto={aberto} />
          <SidebarItem text="Idioma" link="/idioma" logo={<FaLanguage />} mostrarTexto={aberto} />
          <SidebarItem text="Exercícios" link="/exercicio" logo={<GrNotes />} mostrarTexto={aberto} />
          
          <SidebarItem text="Ranking" link="/ranking" logo={<FaArrowUp />} mostrarTexto={aberto} />

        </div>
      </motion.div>

      <div
        className={style.page_content}
        style={{ marginLeft: aberto ? 250 : 60, transition: "margin-left 0.3s ease" }}
      >
        {children}
      </div>
    </div>
  );
}
