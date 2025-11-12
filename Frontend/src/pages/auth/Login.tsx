import axios from 'axios';
import { useState } from 'react';

export default function Login() {
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');

  const handleLogin = async () => {
    try {
      const res = await axios.post('http://localhost:5221/api/auth/login', { email, password });
      const { token, role } = res.data;

      localStorage.setItem('gunter_token', token);
      localStorage.setItem('gunter_role', role);

      if (role === 'seller') {
        window.location.href = 'http://localhost:5174/dashboard';
      } else {
        window.location.href = '/dashboard';
      }
    } catch (err) {
      alert('Error al iniciar sesión');
    }
  };

  return (
    <div className="p-8">
      <h1>Login - Bar Gunter</h1>
      <input placeholder="Email" onChange={e => setEmail(e.target.value)} className="border p-2" />
      <input placeholder="Contraseña" type="password" onChange={e => setPassword(e.target.value)} className="border p-2" />
      
      <button onClick={handleLogin} className="bg-blue-600 text-white px-6 py-2 mt-4">
        Iniciar Sesión
      </button>
    </div>
  );
}