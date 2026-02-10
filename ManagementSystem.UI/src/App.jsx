import { useState } from 'react';
import Login from './pages/login';
import StudentList from './pages/StudentList';

function App() {
  const [isAuthenticated, setIsAuthenticated] = useState(!!localStorage.getItem('token'));

  if (!isAuthenticated) {
    return <Login onLoginSuccess={() => setIsAuthenticated(true)} />;
  }

  return <StudentList onLogout={() => {
    localStorage.removeItem('token');
    setIsAuthenticated(false);
  }} />;
}

export default App;