import style from "./SidebarItem.module.css";
import { Link } from 'react-router-dom';

export function SidebarItem({ text, link, logo, mostrarTexto }) {
    return (
        <Link to={link} className={style.sidebar_item}>
            <div className={style.icon}>{logo}</div>
            {mostrarTexto && <h3 className={style.text_link}>{text}</h3>}
        </Link>
    );
}
