import { useState } from 'react';
import Login from './pages/Login';
import StudentList from './pages/StudentList';

function App() {
    const [isLoggedIn, setIsLoggedIn] = useState(!!localStorage.getItem('token'));

    if (!isLoggedIn) {
        return <Login onLoginSuccess={() => setIsLoggedIn(true)} />;
    }

    return (
        <div className="App">
            <button onClick={() => { localStorage.removeItem('token'); setIsLoggedIn(false); }}>Déconnexion</button>
            <StudentList />
        </div>
    );
}

export default App;