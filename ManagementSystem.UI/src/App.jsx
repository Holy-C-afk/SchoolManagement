import { useState } from 'react';
import { BrowserRouter as Router, Routes, Route, Link, Navigate } from 'react-router-dom';
import Login from './pages/login';
import StudentList from './pages/StudentList';
import TeacherList from './pages/TeacherList'; // Importe ta liste de profs
import Departments from './pages/Departments'; // Importe ta page départements

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('token'));

  const handleLogout = () => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
  };

  if (!isAuthenticated) {
    return <Login onLoginSuccess={() => setIsAuthenticated(true)} />;
  }

  return (
    <Router>
      <div className="min-h-screen bg-gray-50">
        {/* BARRE DE NAVIGATION COMMUNE */}
        <nav className="bg-blue-600 text-white shadow-lg p-4">
          <div className="container mx-auto flex justify-between items-center">
            <div className="flex gap-6">
              <Link to="/students" className="hover:text-blue-200 font-medium">Étudiants</Link>
              <Link to="/teachers" className="hover:text-blue-200 font-medium">Enseignants</Link>
              <Link to="/departments" className="hover:text-blue-200 font-medium">Départements</Link>
            </div>
            <button 
              onClick={handleLogout}
              className="bg-red-500 hover:bg-red-600 px-4 py-2 rounded text-sm transition"
            >
              Déconnexion
            </button>
          </div>
        </nav>

        {/* ZONE DE CONTENU DYNAMIQUE */}
        <div className="container mx-auto py-6">
          <Routes>
            {/* Redirection par défaut vers la liste des étudiants */}
            <Route path="/" element={<Navigate to="/students" />} />
            
            <Route path="/students" element={<StudentList />} />
            <Route path="/teachers" element={<TeacherList />} />
            <Route path="/departments" element={<Departments />} />
            
            {/* Page 404 si l'URL n'existe pas */}
            <Route path="*" element={<div className="p-10 text-center">Page non trouvée</div>} />
          </Routes>
        </div>
      </div>
    </Router>
  );
}

export default App;