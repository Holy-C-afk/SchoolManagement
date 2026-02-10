import { useState } from 'react';
import api from '../api/api';
import { LogIn, User, Lock, UserPlus } from 'lucide-react';

const Login = ({ onLoginSuccess }) => {
    const [credentials, setCredentials] = useState({ username: '', password: '' });
    const [loading, setLoading] = useState(false);
    const [error, setError] = useState('');
    const [isRegisterMode, setIsRegisterMode] = useState(false);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setLoading(true);
        setError('');

        try {
            if (isRegisterMode) {
                await api.post('/Auth/register', credentials);
                setIsRegisterMode(false);
                setError('');
                alert("Compte créé avec succès. Vous pouvez maintenant vous connecter.");
            } else {
                const response = await api.post('/Auth/login', credentials);
                const token = response.data.token || response.data.Token;
                if (!token) {
                    throw new Error("Token manquant dans la réponse du serveur");
                }
                localStorage.setItem('token', token);
                onLoginSuccess();
            }
        } catch {
            setError(isRegisterMode ? "Erreur lors de l'inscription" : "Identifiants invalides");
        } finally {
            setLoading(false);
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center bg-gradient-to-br from-blue-50 to-slate-100 px-4">
            <div className="bg-white w-full max-w-md rounded-3xl shadow-2xl p-8 animate-in fade-in zoom-in duration-300">
                
                <div className="flex flex-col items-center mb-6">
                    <div className="bg-blue-100 text-blue-600 p-4 rounded-2xl mb-3">
                        {isRegisterMode ? <UserPlus size={28} /> : <LogIn size={28} />}
                    </div>
                    <h2 className="text-2xl font-bold text-slate-800">
                        {isRegisterMode ? "Inscription" : "Connexion"}
                    </h2>
                    <p className="text-slate-500 text-sm">
                        {isRegisterMode ? "Créez un compte administrateur" : "Accédez à votre espace"}
                    </p>
                </div>

                <form onSubmit={handleSubmit} className="space-y-4">
                    
                    <div className="relative">
                        <User className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
                        <input
                            type="text"
                            placeholder="Nom d'utilisateur"
                            required
                            className="w-full pl-10 pr-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition"
                            onChange={e => setCredentials({ ...credentials, username: e.target.value })}
                        />
                    </div>

                    <div className="relative">
                        <Lock className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400" />
                        <input
                            type="password"
                            placeholder="Mot de passe"
                            required
                            className="w-full pl-10 pr-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition"
                            onChange={e => setCredentials({ ...credentials, password: e.target.value })}
                        />
                    </div>

                    {error && (
                        <div className="text-sm text-rose-600 bg-rose-50 p-3 rounded-xl">
                            {error}
                        </div>
                    )}

                    <button
                        type="submit"
                        disabled={loading}
                        className="w-full bg-blue-600 text-white py-3 rounded-xl font-bold hover:bg-blue-700 transition-all shadow-lg shadow-blue-200 disabled:opacity-60"
                    >
                        {loading 
                            ? (isRegisterMode ? "Création du compte..." : "Connexion...") 
                            : (isRegisterMode ? "Créer un compte" : "Se connecter")}
                    </button>
                </form>

                <div className="mt-4 text-center text-sm text-slate-500">
                    {isRegisterMode ? (
                        <>
                            Déjà un compte ?{" "}
                            <button
                                type="button"
                                onClick={() => setIsRegisterMode(false)}
                                className="text-blue-600 hover:underline font-medium"
                            >
                                Se connecter
                            </button>
                        </>
                    ) : (
                        <>
                            Pas de compte ?{" "}
                            <button
                                type="button"
                                onClick={() => setIsRegisterMode(true)}
                                className="text-blue-600 hover:underline font-medium"
                            >
                                Créer un compte
                            </button>
                        </>
                    )}
                </div>
            </div>
        </div>
    );
};

export default Login;
