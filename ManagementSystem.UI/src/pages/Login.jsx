import { useState } from 'react';
import api from '../api/api';

const Login = ({ onLoginSuccess }) => {
    const [credentials, setCredentials] = useState({ username: '', password: '' });

    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            const response = await api.post('/Auth/login', credentials);
            localStorage.setItem('token', response.data.token);
            onLoginSuccess();
        } catch (error) {
            alert("Identifiants invalides");
        }
    };

    return (
        <div className="login-container">
            <h2>Connexion au Système</h2>
            <form onSubmit={handleSubmit}>
                <input type="text" placeholder="Nom d'utilisateur" 
                    onChange={e => setCredentials({...credentials, username: e.target.value})} />
                <input type="password" placeholder="Mot de passe" 
                    onChange={e => setCredentials({...credentials, password: e.target.value})} />
                <button type="submit">Se connecter</button>
            </form>
        </div>
    );
};

export default Login;