import React, { useEffect, useState } from "react";
import { Navigate } from "react-router-dom";

export function RequireAdmin({ children }) {
  const [loading, setLoading] = React.useState(true);
  const [isAdmin, setIsAdmin] = React.useState(false);

  useEffect(() => {
    const verificarAdmin = async () => {
      const id = localStorage.getItem("idUsuario");
      const token = localStorage.getItem("token");

      if (!id || !token) {
        setIsAdmin(false);
        setLoading(false);
        return;
      }

      try {
        const res = await fetch(`https://localhost:7076/Usuarios/ObterPorId/${id}`, {
          headers: { Authorization: `Bearer ${token}` },
        });

        if (!res.ok) throw new Error("Erro ao obter usuário");

        const data = await res.json();
        setIsAdmin(data.tipoUsuario === 1);
      } catch (err) {
        setIsAdmin(false);
      } finally {
        setLoading(false);
      }
    };

    verificarAdmin();
  }, []);

  if (loading) {
    return <div>Carregando...</div>; // Ou um spinner
  }

  if (!isAdmin) {
    return <Navigate to="/" replace />; // Redireciona para Home ou login
  }

  return children;
}
