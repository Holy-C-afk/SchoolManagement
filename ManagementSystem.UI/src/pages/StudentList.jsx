import { useEffect, useState } from 'react';
import api from '../api/api';

const StudentList = () => {
    const [students, setStudents] = useState([]);

    useEffect(() => {
    api.get('/Students')
        .then(res => {
            console.log("Données reçues :", res.data); // Regarde dans la console F12
            setStudents(res.data);
        })
        .catch(err => console.error(err));
}, []);

    return (
        <div>
            <h1>Liste des Étudiants</h1>
            <table>
                <thead>
                    <tr>
                        <th>Nom</th>
                        <th>Prénom</th>
                        <th>Email</th>
                    </tr>
                </thead>
                <tbody>
                    <tbody>
  {students.map((s) => (
    <tr key={s.id}>
      {/* On utilise fullName car c'est ce que l'API envoie */}
      <td>{s.fullName}</td>
      <td>{s.birthDate}</td>
      {/* Si tu n'as pas d'email dans l'objet, tu peux mettre un tiret ou une autre info */}
      <td>{s.exams?.length || 0} examens</td> 
    </tr>
  ))}
</tbody>
                </tbody>
            </table>
        </div>
    );
};

export default StudentList;