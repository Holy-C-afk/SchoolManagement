import axios from 'axios';

const api = axios.create({
  baseURL: 'http://localhost:5038/api', // HTTP local pour dev
});

// Interceptor pour ajouter le JWT à chaque requête
api.interceptors.request.use(config => {
  const token = localStorage.getItem('token'); // récupère le token existant
  if (token) {
    config.headers.Authorization = `Bearer ${token}`; // ajoute le header
  }
  return config;
});

export default api;
