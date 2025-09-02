import style from "./PracticeCard.module.css";

export function PracticeCard({ label, iconUrl, children }) {
  return (
    <div className={style.card}>
      <img src={iconUrl} alt={label} className={style.icon} />
      <strong>{label}</strong>
      {children}
      
    </div>
  );
}
