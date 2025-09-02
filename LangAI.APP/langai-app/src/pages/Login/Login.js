// Login.jsx
import style from "./Login.module.css";
import { FaEnvelope, FaLock, FaExclamationCircle } from "react-icons/fa";
import { Link, useNavigate } from "react-router-dom";
import { AuthSidebar } from "../../components/AuthSidebar/AuthSidebar";
import { useForm } from "react-hook-form";
import { yupResolver } from "@hookform/resolvers/yup";
import * as yup from "yup";
import { toast } from "react-toastify";
import axios from "axios";
import { useEffect } from "react";

const schema = yup.object().shape({
  email: yup.string().email("E-mail inválido").required("E-mail é obrigatório"),
  senha: yup
    .string()
    .min(6, "Mínimo 6 caracteres")
    .required("Senha é obrigatória"),
});

export function Login() {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm({
    resolver: yupResolver(schema),
    mode: "onBlur",
  });

  const navigate = useNavigate();

  useEffect(() => {
    const testS3Connection = async () => {
      try {
        const testUrl =
          "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/test.txt";
        const res = await fetch(testUrl, { method: "HEAD" });
        console.log("Bucket S3 acessível?", res.ok);
      } catch (err) {
        console.error("Falha ao testar conexão com S3:", err);
      }
    };

    testS3Connection();
  }, []);

  const onSubmit = async (formData) => {
    try {
      const payload = {
        Email: formData.email,
        Senha: formData.senha,
      };

      const response = await axios.post(
        "https://localhost:7076/Usuarios/login",
        payload
      );

      const data = response.data;

      // Salva dados no localStorage
      localStorage.setItem("token", data.token);
      localStorage.setItem("nomeUsuario", data.nome);
      localStorage.setItem("idUsuario", data.usuarioID);
      localStorage.setItem("tipoUsuario", data.tipoUsuario); // <- agora salva número

      toast.success("Login realizado com sucesso!");
      reset();

      // Redireciona baseado no tipo numérico
      if (data.tipoUsuario === 1) {
        navigate("/admin");
      } else {
        navigate("/home"); // rota do usuário normal
      }
    } catch (error) {
      console.error(error);
      toast.error(
        error.response?.data || "Erro ao fazer login. Verifique os dados."
      );
    }
  };

  const s3ImageUrl =
    "https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/background/login1.svg";

  return (
    <div className={style.container}>
      <div className={style.sidebar}>
        <AuthSidebar illustration={s3ImageUrl} />
      </div>

      <div className={style.imageSection}>
        <img src={s3ImageUrl} alt="Ilustração de login" />
      </div>

      <div className={style.formSection}>
        <form className={style.form} onSubmit={handleSubmit(onSubmit)}>
          <p className={style.cadastrar}>
            Ainda não tem uma conta?{" "}
            <Link to="/cadastro" className="route-link">
              Cadastrar.
            </Link>
          </p>

          <label>
            <span>E-mail</span>
            <div
              className={`${style.inputGroup} ${
                errors.email ? style.inputError : ""
              }`}
            >
              <input
                type="email"
                placeholder="Digite seu e-mail"
                {...register("email")}
              />
              <FaEnvelope className={style.icon} />
            </div>
            {errors.email && (
              <div className={style.errorContainer}>
                <FaExclamationCircle className={style.errorIcon} />
                <span className={style.errorMessage}>
                  {errors.email.message}
                </span>
              </div>
            )}
          </label>

          <label>
            <span>Senha</span>
            <div
              className={`${style.inputGroup} ${
                errors.senha ? style.inputError : ""
              }`}
            >
              <input
                type="password"
                placeholder="Digite sua senha"
                {...register("senha")}
              />
              <FaLock className={style.icon} />
            </div>
            {errors.senha && (
              <div className={style.errorContainer}>
                <FaExclamationCircle className={style.errorIcon} />
                <span className={style.errorMessage}>
                  {errors.senha.message}
                </span>
              </div>
            )}
          </label>

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? "Entrando..." : "Entrar"}
          </button>
        </form>
      </div>
    </div>
  );
}
