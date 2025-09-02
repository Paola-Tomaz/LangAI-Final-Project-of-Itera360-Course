import style from './Cadastro.module.css';
import { Link } from 'react-router-dom';
import { FaEnvelope, FaLock, FaUser, FaExclamationCircle } from 'react-icons/fa';
import { AuthSidebar } from '../../components/AuthSidebar/AuthSidebar';
import { useForm } from 'react-hook-form';
import { yupResolver } from '@hookform/resolvers/yup';
import * as yup from 'yup';
import axios from 'axios';
import { toast } from 'react-toastify';

const schema = yup.object().shape({
  nome: yup.string().required('Nome é obrigatório'),
  email: yup.string().email('E-mail inválido').required('E-mail é obrigatório'),
  senha: yup.string().min(6, 'Mínimo 6 caracteres').required('Senha é obrigatória'),
  termos: yup.boolean().oneOf([true], 'Você precisa aceitar os termos'),
});

function adicionarAoRanking(nome) {
  const rankingSalvo = JSON.parse(localStorage.getItem('ranking')) || [];
  const existe = rankingSalvo.find((item) => item.nome === nome);
  if (!existe) {
    rankingSalvo.push({ nome, xp: 0 });
    localStorage.setItem('ranking', JSON.stringify(rankingSalvo));
  }
}

export function Cadastro() {
  const {
    register,
    handleSubmit,
    formState: { errors, isSubmitting },
    reset,
  } = useForm({
    resolver: yupResolver(schema),
    mode: 'onBlur',
  });

  const onSubmit = async (data) => {
    try {
      const payload = {
        nome: data.nome,
        email: data.email,
        senha: data.senha,
        idiomaSelecionadoCodigo: 'pt-BR',
        tipoUsuario: 0,
      };

      await axios.post('https://localhost:7076/Usuarios/Criar', payload);

      toast.success('Conta criada com sucesso!');
      adicionarAoRanking(data.nome);
      reset();
    } catch (error) {
      console.error(error);
      toast.error('Erro ao criar conta. Verifique os dados e tente novamente.');
    }
  };

  const s3ImageUrl = 'https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/background/sign-up1.svg';

  return (
    <div className={style.container}>
      <div className={style.sidebar}>
        <AuthSidebar illustration={s3ImageUrl} />
      </div>

      <div className={style.imageSection}>
        <img src={s3ImageUrl} alt="Ilustração de cadastro" />
      </div>

      <div className={style.formSection}>
        <form className={style.form} onSubmit={handleSubmit(onSubmit)}>
          <p className={style.loginLink}>
            Já tem uma conta? <Link to="/login" className="route-link">Entrar.</Link>
          </p>

          <label>
            <span>Nome</span>
            <div className={`${style.inputGroup} ${errors.nome ? style.inputError : ''}`}>
              <input type="text" placeholder="Digite seu nome" {...register('nome')} />
              <FaUser className={style.icon} />
            </div>
            {errors.nome && (
              <div className={style.errorContainer}>
                <FaExclamationCircle className={style.errorIcon} />
                <span className={style.errorMessage}>{errors.nome.message}</span>
              </div>
            )}
          </label>

          <label>
            <span>E-mail</span>
            <div className={`${style.inputGroup} ${errors.email ? style.inputError : ''}`}>
              <input type="email" placeholder="Digite seu e-mail" {...register('email')} />
              <FaEnvelope className={style.icon} />
            </div>
            {errors.email && (
              <div className={style.errorContainer}>
                <FaExclamationCircle className={style.errorIcon} />
                <span className={style.errorMessage}>{errors.email.message}</span>
              </div>
            )}
          </label>

          <label>
            <span>Senha</span>
            <div className={`${style.inputGroup} ${errors.senha ? style.inputError : ''}`}>
              <input type="password" placeholder="Digite sua senha" {...register('senha')} />
              <FaLock className={style.icon} />
            </div>
            {errors.senha && (
              <div className={style.errorContainer}>
                <FaExclamationCircle className={style.errorIcon} />
                <span className={style.errorMessage}>{errors.senha.message}</span>
              </div>
            )}
          </label>

          <div className={style.termos}>
            <input type="checkbox" id="termos" {...register('termos')} />
            <label htmlFor="termos">
              Ao criar uma conta você concorda com os <strong>Termos de Uso</strong> e <strong>Política de Privacidade</strong>.
            </label>
          </div>
          {errors.termos && (
            <div className={style.errorContainer}>
              <FaExclamationCircle className={style.errorIcon} />
              <span className={style.errorMessage}>{errors.termos.message}</span>
            </div>
          )}

          <button type="submit" disabled={isSubmitting}>
            {isSubmitting ? 'Cadastrando...' : 'Cadastrar'}
          </button>
        </form>
      </div>
    </div>
  );
}
