import React, { useState, useEffect, useCallback } from "react";
import { useNavigate } from "react-router-dom";
import api from "../../config/axiosConfig";
import styles from "./Exercicio.module.css";
import { Sidebar } from "../../components/Sidebar/Sidebar";
import { Topbar } from "../../components/Topbar/Topbar";
import {
  FaArrowRight,
  FaArrowAltCircleRight,
  FaRegSmile,
  FaRegFrown,
} from "react-icons/fa";
import PageTitle from "../../components/PageTitle/PageTitle";

// Componente CircularProgress separado
const CircularProgress = ({ progress }) => {
  const radius = 50;
  const stroke = 8;
  const normalizedRadius = radius - stroke * 2;
  const circumference = normalizedRadius * 2 * Math.PI;
  const strokeDashoffset =
    circumference - (progress / 100) * circumference;

  return (
    <svg height={radius * 2} width={radius * 2}>
      <circle
        stroke="#eee"
        fill="transparent"
        strokeWidth={stroke}
        r={normalizedRadius}
        cx={radius}
        cy={radius}
      />
      <circle
        stroke="#255736" // cor da barra
        fill="transparent"
        strokeWidth={stroke}
        strokeLinecap="round"
        strokeDasharray={`${circumference} ${circumference}`}
        style={{ strokeDashoffset, transition: "stroke-dashoffset 0.5s ease" }}
        r={normalizedRadius}
        cx={radius}
        cy={radius}
      />
      <text
        x="50%"
        y="50%"
        dy="0.3em"
        textAnchor="middle"
        fontSize="20"
        fill="#255736"
        fontWeight="bold"
      >
        {progress}%
      </text>
    </svg>
  );
};

const Exercicio = () => {
  const personagem = JSON.parse(localStorage.getItem("personagemSelecionado"));
  const navigate = useNavigate();
  const usuarioID = Number(localStorage.getItem("idUsuario"));
  const token = localStorage.getItem("token");

  const [contadorPerguntas, setContadorPerguntas] = useState(0);
  const [finalizou, setFinalizou] = useState(false);
  const [selecionada, setSelecionada] = useState(null);
  const [respondido, setRespondido] = useState(false);
  const [xp, setXp] = useState(0);
  const [progresso, setProgresso] = useState(0);
  const [acertou, setAcertou] = useState(null);
  const [perguntaAtual, setPerguntaAtual] = useState(null);
  const [carregando, setCarregando] = useState(true);
  const [erro, setErro] = useState(null);
  const [personagemFala, setPersonagemFala] = useState("Vamos aprender juntos!");

  const idiomaMap = {
    alemão: "alemao",
    inglês: "ingles",
    francês: "frances",
    japonês: "japones",
  };

  const extrairPergunta = (data) => {
    if (
      data &&
      data.pergunta &&
      typeof data.pergunta === "object" &&
      "pergunta" in data.pergunta &&
      "opcoes" in data.pergunta &&
      "respostaCorreta" in data.pergunta
    ) {
      return data.pergunta;
    }
    return null;
  };

  const gerarPergunta = useCallback(async () => {
    setCarregando(true);
    setErro(null);
    setPersonagemFala("Estou pensando em uma pergunta...");

    try {
      const idiomaSelecionado =
        localStorage.getItem("idiomaSelecionado") || "inglês";
      const idiomaAPI = idiomaMap[idiomaSelecionado] || "english";
      const resposta = await api.post(`/ai/gerar?idioma=${idiomaAPI}`);
      const perguntaExtraida = extrairPergunta(resposta.data);
      if (!perguntaExtraida) throw new Error("Formato inválido");
      setPerguntaAtual(perguntaExtraida);
      setPersonagemFala("Tente responder esta!");
    } catch (error) {
      console.error("Erro ao gerar pergunta:", error);
      setErro("Erro ao carregar. Usando pergunta padrão.");
      setPerguntaAtual({
        pergunta: 'Como se diz "Boa tarde" em inglês?',
        opcoes: ["Good morning", "Good night", "Good afternoon", "Hello"],
        respostaCorreta: "Good afternoon",
      });
      setPersonagemFala("Vamos com uma pergunta padrão!");
    } finally {
      setCarregando(false);
    }
  }, []);

  const buscarXP = useCallback(async () => {
    try {
      const resposta = await api.get(`/usuarios/ObterPorId/${usuarioID}`, {
        headers: { Authorization: `Bearer ${token}` },
      });
      const xpAtual = resposta.data?.XP ?? 0;
      setXp(xpAtual);
      console.log("XP atualizado:", xpAtual);
    } catch (error) {
      console.error("Erro ao buscar XP do usuário:", error);
      setXp(0);
    }
  }, [usuarioID, token]);

  useEffect(() => {
    if (!usuarioID || isNaN(usuarioID) || !token) {
      navigate("/login");
      return;
    }

    if (!localStorage.getItem("idiomaSelecionado")) {
      navigate("/idioma");
      return;
    }

    buscarXP();
    gerarPergunta();
  }, [usuarioID, token, gerarPergunta, navigate, buscarXP]);

  const limparTexto = (texto) =>
    texto ? texto.replace(/^[A-D]\.\s*/, "").toLowerCase() : "";

  const verificarResposta = async (opcao) => {
    if (respondido || carregando || finalizou) return;

    setSelecionada(opcao);
    setRespondido(true);

    const corretaLimpa = limparTexto(perguntaAtual?.respostaCorreta);
    const opcaoLimpa = limparTexto(opcao);
    const correta = opcaoLimpa === corretaLimpa;
    setAcertou(correta);

    setProgresso((prev) => Math.min(prev + 20, 100));

    if (correta) {
      setXp((prevXp) => prevXp + 20); // Atualiza imediatamente localmente
      setPersonagemFala("Isso aí! Você acertou! 🎉");
      try {
        await api.post(
          "/Usuarios/atualizarXP",
          { XP: 20 },
          {
            headers: {
              Authorization: `Bearer ${token}`,
              "Content-Type": "application/json",
            },
          }
        );
        // Não chama buscarXP para evitar sobrescrever com dados desatualizados
      } catch (error) {
        console.error("Erro ao atualizar XP:", error);
        setErro("Erro ao atualizar XP. Tente novamente.");
      }
    } else {
      setPersonagemFala(
        `Ops! A resposta correta era: ${perguntaAtual?.respostaCorreta}`
      );
    }
  };

  const proximaPergunta = () => {
    if (contadorPerguntas + 1 >= 5) {
      setFinalizou(true);
      setPersonagemFala("Parabéns! Você concluiu o exercício!");
      return;
    }

    setSelecionada(null);
    setRespondido(false);
    setAcertou(null);
    setContadorPerguntas((prev) => prev + 1);
    setPersonagemFala("Próximo desafio...");
    gerarPergunta();
  };

  const pularPergunta = () => {
    setPersonagemFala("Vamos para outra então!");
    proximaPergunta();
  };

  const reiniciarExercicio = () => {
    setContadorPerguntas(0);
    setProgresso(0);
    setFinalizou(false);
    setRespondido(false);
    setSelecionada(null);
    setPerguntaAtual(null);
    setPersonagemFala("Vamos começar de novo!");
    gerarPergunta();
  };

  return (
    <Sidebar>
      <Topbar>
        <div className={styles.container}>
          <PageTitle>Exercícios</PageTitle>
          <div className={styles.mainContent}>
            <div className={styles.contentWrapper}>
              <div className={styles.content}>
                <div className={styles.progressContainer}>
                  <div
                    className={styles.progressBar}
                    style={{ width: `${progresso}%` }}
                  ></div>
                  <span>{progresso}% completo</span>
                </div>
                {erro && <p className={styles.errorMessage}>{erro}</p>}
                {finalizou ? (
                  <div className={styles.fimExercicio}>
                    <h2>🎉 Você concluiu o exercício!</h2>
                    <p>Total de XP ganho: {xp}</p>
                    <button
                      className={styles.nextButton}
                      onClick={reiniciarExercicio}
                    >
                      <FaArrowAltCircleRight /> Recomeçar
                    </button>
                  </div>
                ) : (
                  <>
                    <div className={styles.card}>
                      <div className={styles.character}>
                        <img
                          src={personagem?.imagem || "/assets/img/padrao.png"}
                          alt={personagem?.nome || "Professor"}
                          className={styles.characterImage}
                        />
                        <div className={styles.characterInfo}>
                          <p className={styles.personagemNome}>
                            Sou {personagem?.nome}, vamos nessa!
                          </p>
                          <div
                            className={styles.speechBubble}
                            style={{ borderColor: personagem?.cor }}
                          >
                            <p className={styles.pergunta}>
                              {perguntaAtual?.pergunta || "Carregando pergunta..."}
                            </p>
                            <p className={styles.personagemFala}>{personagemFala}</p>
                          </div>
                        </div>
                      </div>
                    </div>

                    <div className={styles.options}>
                      {Array.isArray(perguntaAtual?.opcoes) &&
                        perguntaAtual.opcoes.map((opcao, i) => (
                          <button
                            key={i}
                            className={`${styles.option} ${
                              respondido
                                ? opcao === perguntaAtual.respostaCorreta
                                  ? styles.correct
                                  : opcao === selecionada
                                  ? styles.incorrect
                                  : ""
                                : ""
                            }`}
                            onClick={() => verificarResposta(opcao)}
                            disabled={respondido || carregando}
                          >
                            {opcao}
                          </button>
                        ))}
                    </div>

                    {respondido && (
                      <div
                        className={
                          acertou
                            ? styles.feedbackCorrect
                            : styles.feedbackIncorrect
                        }
                      >
                        {acertou ? (
                          <>
                            <FaRegSmile className={styles.feedbackIcon} />
                            <span>+20 XP! Continue assim!</span>
                          </>
                        ) : (
                          <>
                            <FaRegFrown className={styles.feedbackIcon} />
                            <span>
                              Resposta correta: {perguntaAtual?.respostaCorreta}
                            </span>
                          </>
                        )}
                      </div>
                    )}

                    <div className={styles.actions}>
                      <button
                        className={styles.skipButton}
                        onClick={pularPergunta}
                        disabled={carregando}
                      >
                        <FaArrowRight /> Pular
                      </button>
                      <button
                        className={styles.nextButton}
                        onClick={proximaPergunta}
                        disabled={!respondido || carregando}
                      >
                        <FaArrowAltCircleRight /> Continuar
                      </button>
                    </div>
                  </>
                )}
              </div>

              <div className={styles.scorePanel}>
                <h3>Seu Progresso</h3>
                <div className={styles.xpDisplay}>
                  <span>{xp}</span>
                  <span className={styles.xpLabel}>XP</span>
                </div>

                {/* Barra de XP horizontal */}
                <div className={styles.xpProgressContainer}>
                  <div className={styles.xpProgress}>
                    <div
                      className={styles.xpBar}
                      style={{ width: `${xp % 100}%` }}
                    ></div>
                  </div>
                  <div className={styles.xpProgressText}>{xp % 100}/100 XP</div>
                </div>

                {/* Gráfico circular */}
                <div style={{ width: 120, height: 120, margin: "1rem auto" }}>
                  <CircularProgress progress={xp % 100} />
                </div>

                <div className={styles.levelInfo}>
                  <div className={styles.currentLevel}>
                    Nível: {Math.floor(xp / 100) + 1}
                  </div>
                  <div className={styles.nextLevel}>
                    Próximo nível em: {100 - (xp % 100)} XP
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </Topbar>
    </Sidebar>
  );
};

export default Exercicio;
