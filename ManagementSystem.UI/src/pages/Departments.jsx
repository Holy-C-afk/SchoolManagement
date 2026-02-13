import React, { useEffect, useState } from 'react';
import api from "../api/api";

const Departments = () => {
  const [departments, setDepartments] = useState([]);
  const [newName, setNewName] = useState('');

  const fetchDepartments = async () => {
    const res = await api.get('/Departments');
    setDepartments(res.data);
  };

  const handleCreate = async (e) => {
  e.preventDefault();
  
  // VÉRIFICATION : Supprime les espaces et vérifie si c'est vide
  if (!newName.trim()) {
    alert("Le nom du département ne peut pas être vide !");
    return;
  }

  try {
    await api.post('/Departments', { name: newName });
    setNewName('');
    fetchDepartments();
  } catch (err) { 
    alert("Erreur lors de la création"); 
  }
};

  useEffect(() => { fetchDepartments(); }, []);

  return (
    <div className="p-6">
      <h2 className="text-2xl font-bold mb-4">Gestion des Départements</h2>
      <form onSubmit={handleCreate} className="mb-6 flex gap-2">
        <input 
          className="border p-2 rounded w-64"
          value={newName} 
          onChange={(e) => setNewName(e.target.value)}
          placeholder="Nom du département (ex: Informatique)"
        />
        <button className="bg-blue-500 text-white px-4 py-2 rounded">Ajouter</button>
      </form>
      <ul className="bg-white shadow rounded-lg divide-y">
        {departments.map(d => (
          <li key={d.id} className="p-3">{d.name}</li>
        ))}
      </ul>
    </div>
  );
};

export default Departments;