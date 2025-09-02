import React, { useEffect, useState } from "react";
import styles from "./Progresso.module.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import PageTitle from "../../components/PageTitle/PageTitle";
import api from "../../config/axiosConfig";
import {
  PieChart,
  Pie,
  Cell,
  BarChart,
  Bar,
  XAxis,
  YAxis,
  Tooltip,
  ResponsiveContainer,
} from "recharts";

const COLORS = ["#255736", "#FFD8C3", "#EEB04D", "#562F13"];

const Progresso = () => {
  const [dadosXP, setDadosXP] = useState(null);
  const [xpTotal, setXpTotal] = useState(0);

  const usuarioID = localStorage.getItem("idUsuario");
  const token = localStorage.getItem("token");

  useEffect(() => {
    const buscarXP = async () => {
      try {
        const resposta = await api.get(`/Usuarios/progresso/${usuarioID}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        const dados = resposta.data;
        const dadosFormatados = {
          ingles: dados.XPIngles || 0,
          japones: dados.XPJapones || 0,
          frances: dados.XPFrances || 0,
          alemao: dados.XPAlemao || 0,
        };
        setDadosXP(dadosFormatados);

        const totalXP = Object.values(dadosFormatados).reduce(
          (acc, val) => acc + val,
          0
        );
        setXpTotal(totalXP);
      } catch (erro) {
        console.error("Erro ao buscar progresso:", erro);
      }
    };

    buscarXP();
  }, [usuarioID, token]);

  const dataBarras = dadosXP
    ? Object.entries(dadosXP).map(([idioma, valor]) => ({
        idioma: idioma.charAt(0).toUpperCase() + idioma.slice(1),
        XP: valor,
      }))
    : [];

  const dataCirculo = [
    { name: "XP Atual", value: xpTotal % 100 },
    { name: "Faltam", value: 100 - (xpTotal % 100) },
  ];

  const nivelAtual = Math.floor(xpTotal / 100) + 1;

  return (
    <Sidebar>
      <Topbar>
        <div className={styles.container}>
          <PageTitle>Seu Progresso</PageTitle>

          <div className={styles.graficos}>
            <div className={styles.graficoCircular}>
              <h3>Nível Atual</h3>
              <ResponsiveContainer width="100%" height={250}>
                <PieChart>
                  <Pie
                    data={dataCirculo}
                    innerRadius={60}
                    outerRadius={80}
                    paddingAngle={5}
                    dataKey="value"
                  >
                    {dataCirculo.map((entry, index) => (
                      <Cell
                        key={`cell-${index}`}
                        fill={index === 0 ? "#255736" : "#eee"}
                      />
                    ))}
                  </Pie>
                </PieChart>
              </ResponsiveContainer>
              <div className={styles.nivelTexto}>
                Nível: <strong>{nivelAtual}</strong> <br />
                {xpTotal % 100}/100 XP
              </div>
            </div>

            <div className={styles.graficoBarras}>
              <h3>XP por Idioma</h3>
              <ResponsiveContainer width="100%" height={250}>
                <BarChart data={dataBarras}>
                  <XAxis dataKey="idioma" />
                  <YAxis />
                  <Tooltip />
                  <Bar dataKey="XP" fill="#EEB04D" />
                </BarChart>
              </ResponsiveContainer>
            </div>
          </div>
        </div>
      </Topbar>
    </Sidebar>
  );
};

export default Progresso;
