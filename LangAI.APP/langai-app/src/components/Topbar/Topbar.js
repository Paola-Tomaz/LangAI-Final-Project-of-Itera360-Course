import style from "./Topbar.module.css";
import { useNavigate } from "react-router-dom";
import { FiLogOut } from "react-icons/fi";

export function Topbar({ children }) {
  const navigate = useNavigate();

  const handleLogout = () => {
    localStorage.removeItem("token");
    localStorage.removeItem("idUsuario");
    navigate("/login");
  };

  return (
    <div>
      <div className={style.topbar_content}>
        <div className={style.spacer} /> {/* isso empurra o botão para a direita */}
        <button className={style.logoutButton} onClick={handleLogout}>
          <FiLogOut size={18} />
          <span className={style.logoutText}>Sair</span>
        </button>
      </div>
      <div className={style.page_content}>{children}</div>
    </div>
  );
}
