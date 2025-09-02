import { useEffect, useState } from 'react';
import axios from 'axios';
import { Navigate } from 'react-router-dom';

export function UsuariosPage() {
  const [usuarios, setUsuarios] = useState([]);

  useEffect(() => {
    const token = localStorage.getItem('jwtToken');

    axios.get('https://lang-ai-api.azurewebsites.net/Usuarios/ListarTodos', {
      headers: {
        Authorization: `Bearer ${token}`,
      }
    })
    .then(res => setUsuarios(res.data))
    .catch(err => console.error('Erro ao buscar usuários:', err));
  }, []);

  return (
    <div>
      <h2>Usuários cadastrados</h2>
      <ul>
        {usuarios.map((user) => (
          <li key={user.id}>{user.nome} – {user.email}</li>
        ))}
      </ul>
    </div>
  );
}
