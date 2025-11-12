import axios from 'axios';
import { useState } from 'react';

export default function Register() {
  const [role, setRole] = useState<'customer' | 'seller'>('customer');
  const [email, setEmail] = useState('');
  const [password, setPassword] = useState('');
  const [name, setName] = useState('');

  const handleRegister = async () => {
    try {
      const res = await axios.post('http://localhost:5221/api/auth/register', {
        firstName: name,
        lastName: '',
        email,
        password,
        role  // <-- envías el rol elegido
      });

      const { token, role: userRole } = res.data;

      // GUARDAMOS EL TOKEN EN LOCALSTORAGE (compartido entre los dos frontends)
      localStorage.setItem('gunter_token', token);
      localStorage.setItem('gunter_role', userRole);

      // REDIRECCIÓN SEGÚN ROL
      if (userRole === 'seller') {
        window.location.href = 'http://localhost:5174/dashboard'; // MANDA AL SELLER
      } else {
        window.location.href = '/dashboard'; // queda en el cliente normal
      }
    } catch (err) {
      alert('Error al registrarse');
    }
  };

  return (
    <div className="p-8">
      <h1>Registro - Bar Gunter</h1>
      <input placeholder="Nombre" onChange={e => setName(e.target.value)} className="border p-2" />
      <input placeholder="Email" onChange={e => setEmail(e.target.value)} className="border p-2" />
      <input placeholder="Contraseña" type="password" onChange={e => setPassword(e.target.value)} className="border p-2" />
      
      <select value={role} onChange={e => setRole(e.target.value as any)} className="border p-2 mt-4">
        <option value="customer">Cliente Normal</option>
        <option value="seller">Soy Vendedor (tengo bar)</option>
      </select>

      <button onClick={handleRegister} className="bg-blue-600 text-white px-6 py-2 mt-4">
        Registrarme
      </button>
    </div>
  );
}