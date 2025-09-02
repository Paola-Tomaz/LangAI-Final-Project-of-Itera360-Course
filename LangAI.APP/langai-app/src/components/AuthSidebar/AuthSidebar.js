import { Link, useLocation } from "react-router-dom";
import { FaSignInAlt, FaUserPlus } from "react-icons/fa";
import styles from "./AuthSidebar.module.css";
import logo from "../../assets/logo.svg";

export function AuthSidebar({ illustration }) {
  const location = useLocation();

  return (
    <aside className={styles.sidebar}>
      <div className={styles.navSection}>
        <img src="https://meuapp-fotos-perfil.s3.us-east-2.amazonaws.com/Logo/Logo.png"  alt="Logo LangAI" className={styles.logo} />

        <nav className={styles.nav}>
          <Link
            to="/login"
            className={location.pathname === "/login" ? styles.active : ""}
          >
            <FaSignInAlt />
            Login
          </Link>
          <Link
            to="/cadastro"
            className={location.pathname === "/cadastro" ? styles.active : ""}
          >
            <FaUserPlus />
            Cadastro
          </Link>
        </nav>
      </div>

      {illustration && (
        <div className={styles.illustrationBox}>
        </div>
      )}
    </aside>
  );
}
