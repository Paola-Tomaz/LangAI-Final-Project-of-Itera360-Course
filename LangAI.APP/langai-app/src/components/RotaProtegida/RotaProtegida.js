import { Navigate } from 'react-router-dom';

export function RotaProtegida({ children }) {
  const token = localStorage.getItem('token');
  const usuarioId = localStorage.getItem('idUsuario');
  return token && usuarioId ? children : <Navigate to="/login" />
}
