 // src/pages/AuthWrapper.jsx
import { useLocation } from "react-router-dom";
import { motion, AnimatePresence } from "framer-motion";
import { AuthSidebar } from "../components/AuthSidebar/AuthSidebar";
import loginImg from "../assets/login.jpg";
import cadastroImg from "../assets/cadastro.jpg";
import { LoginForm } from "../components/LoginForm";
import { CadastroForm } from "../components/CadastroForm";
import styles from "./AuthWrapper.module.css";

export function AuthWrapper() {
  const location = useLocation();
  const isLogin = location.pathname === "/login";

  return (
    <div className={styles.container}>
      <AuthSidebar illustration={isLogin ? loginImg : cadastroImg} />

      <main className={styles.main}>
        <AnimatePresence mode="wait">
          <motion.div
            key={isLogin ? "login" : "cadastro"}
            initial={{ opacity: 0, x: 50 }}
            animate={{ opacity: 1, x: 0 }}
            exit={{ opacity: 0, x: -50 }}
            transition={{ duration: 0.4 }}
          >
            {isLogin ? <LoginForm /> : <CadastroForm />}
          </motion.div>
        </AnimatePresence>
      </main>
    </div>
  );
}
