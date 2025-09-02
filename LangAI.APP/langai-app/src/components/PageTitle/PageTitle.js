// src/components/PageTitle/PageTitle.js
import React from "react";
import styles from "./PageTitle.module.css";

export default function PageTitle({ children }) {
  return (
    <div className={styles.container}>
      <h1 className={styles.title}>{children}</h1>
      <div className={styles.underline}></div>
    </div>
  );
}