import React, { useEffect, useState } from 'react';
import api from '../api/api';
import { 
    GraduationCap, Trash2, Plus, X, Pencil, Search, 
    ChevronLeft, ChevronRight, FileText, Users 
} from 'lucide-react';

const StudentList = ({ onLogout }) => {
    // États pour les données
    const [students, setStudents] = useState([]);
    const [loading, setLoading] = useState(true);
    const [error, setError] = useState(null);

    // États Pagination & Recherche
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [search, setSearch] = useState('');

    // États Modales
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [isBulkModalOpen, setIsBulkModalOpen] = useState(false);

    // États Formulaires
    const [newStudent, setNewStudent] = useState({ fullName: '', birthDate: '' });
    const [editStudent, setEditStudent] = useState({ id: null, fullName: '', birthDate: '' });
    const [bulkInput, setBulkInput] = useState('');

    // 1. Charger les étudiants
    const fetchStudents = async (page = pageNumber) => {
        try {
            setLoading(true);
            const res = await api.get('/Students/paged', {
                params: { pageNumber: page, pageSize, search }
            });
            setStudents(res.data.items);
            setTotalPages(res.data.totalPages);
            setPageNumber(res.data.pageNumber);
            console.log("Structure des données reçues :", res.data.items[0]);
            setError(null);
        } catch (err) {
            setError("Impossible de charger les étudiants");
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchStudents(1);
    }, [search]);

    // 2. Actions CRUD
    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            const names = newStudent.fullName.trim().split(' ');
            await api.post('/Students', {
                firstName: names[0] || '',
                lastName: names.slice(1).join(' ') || '',
                birthDate: newStudent.birthDate
            });
            setIsModalOpen(false);
            setNewStudent({ fullName: '', birthDate: '' });
            fetchStudents();
        } catch (err) {
            alert("Erreur lors de la création");
        }
    };

    const handleUpdate = async (e) => {

e.preventDefault();

try {

const names = editStudent.fullName.split(' ');

await api.put(`/Students/${editStudent.id}`, {

firstName: names[0] || '',

lastName: names.slice(1).join(' ') || '',

birthDate: editStudent.birthDate

});

setIsEditModalOpen(false);

setEditStudent({ id: null, fullName: '', birthDate: '' });

fetchStudents();

} catch (err) {

console.error("Erreur mise à jour étudiant :", err.response?.data || err.message);

alert(

err.response?.data?.message ||

JSON.stringify(err.response?.data) ||

"Erreur lors de la mise à jour"

);

}

};
    const handleOpenEdit = (student) => {

setEditStudent({

id: student.id,

fullName: student.fullName,

birthDate: (student.birthDate || '').toString().substring(0, 10),

});

setIsEditModalOpen(true);

};

    const handleDelete = async (id) => {
        if (!window.confirm("Supprimer cet étudiant ?")) return;
        try {
            await api.delete(`/Students/${id}`);
            fetchStudents();
        } catch (err) {
            alert("Erreur lors de la suppression");
        }
    };

    const handleBulkCreate = async (e) => {
        e.preventDefault();
        try {
            const lines = bulkInput.split('\n').filter(l => l.trim() !== '');
            const payload = lines.map((line, index) => {
                const [fullName, birthDate] = line.split(';').map(p => p.trim());
                
                if (!fullName || !birthDate) {
                    throw new Error(`Ligne ${index + 1} incomplete. Format: Nom;YYYY-MM-DD`);
                }

                // Validation format date YYYY-MM-DD
                if (!/^\d{4}-\d{2}-\d{2}$/.test(birthDate)) {
                    throw new Error(`Ligne ${index + 1}: Date "${birthDate}" invalide. Utilisez YYYY-MM-DD.`);
                }

                const names = fullName.split(' ');
                return {
                    firstName: names[0],
                    lastName: names.slice(1).join(' '),
                    birthDate: birthDate
                };
            });

            await api.post('/Students/bulk', payload);
            setIsBulkModalOpen(false);
            setBulkInput('');
            fetchStudents();
            alert("Importation réussie !");
        } catch (err) {
            alert(err.message || "Erreur lors de l'ajout multiple");
        }
    };

    const handleExportPdf = async () => {
        try {
            const res = await api.get('/Students/export/pdf', { 
                params: { search },
                responseType: 'blob' 
            });
            const url = window.URL.createObjectURL(new Blob([res.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `Etudiants_${new Date().toISOString().split('T')[0]}.pdf`);
            document.body.appendChild(link);
            link.click();
            link.remove();
        } catch (err) {
            alert("Erreur export PDF");
        }
    };

    return (
        <div className="min-h-screen bg-slate-50 p-4 md:p-8">
            <div className="max-w-6xl mx-auto">
                
                {/* Header */}
                <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-8">
                    <div>
                        <h1 className="text-3xl font-bold text-slate-900 flex items-center gap-3">
                            <GraduationCap className="text-indigo-600 size-10" />
                            Étudiants
                        </h1>
                    </div>
                    
                    <div className="flex flex-col md:flex-row gap-3 items-stretch md:items-center">
                        <div className="relative">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 size-4" />
                            <input
                                type="text"
                                placeholder="Rechercher..."
                                className="w-full md:w-64 pl-9 pr-3 py-2 rounded-xl border border-slate-200 outline-none focus:ring-2 focus:ring-indigo-500 text-sm"
                                value={search}
                                onChange={e => setSearch(e.target.value)}
                            />
                        </div>
                        <button 
                            onClick={() => setIsModalOpen(true)}
                            className="bg-indigo-600 text-white px-5 py-2 rounded-xl font-semibold hover:bg-indigo-700 flex items-center gap-2 shadow-lg shadow-indigo-100 transition-all"
                        >
                            <Plus size={18} /> Nouveau
                        </button>
                        <button 
                            onClick={() => setIsBulkModalOpen(true)}
                            className="bg-emerald-600 text-white px-5 py-2 rounded-xl font-semibold hover:bg-emerald-700 flex items-center gap-2 shadow-lg shadow-emerald-100 transition-all"
                        >
                            <Users size={18} /> Bulk Add
                        </button>
                        <button 
                            onClick={handleExportPdf}
                            className="bg-slate-800 text-white px-4 py-2 rounded-xl hover:bg-slate-900 flex items-center gap-2 transition-all"
                        >
                            <FileText size={18} /> PDF
                        </button>
                        <button onClick={onLogout} className="text-slate-400 hover:text-rose-600 text-sm font-medium px-2 transition-colors">
                            Déconnexion
                        </button>
                    </div>
                </div>

                {/* Table Section */}
                <div className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
                    <table className="w-full text-left border-collapse">
                        <thead className="bg-slate-50 border-b border-slate-100">
                            <tr>
                                <th className="p-4 font-semibold text-slate-600 text-sm uppercase tracking-wider">Nom Complet</th>
                                <th className="p-4 font-semibold text-slate-600 text-sm uppercase tracking-wider">Date de Naissance</th>
                                <th className="p-4 text-right font-semibold text-slate-600 text-sm uppercase tracking-wider">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-100">
    {students.length > 0 ? students.map(s => {
        // On récupère la date peu importe la casse du champ
        const dateValue = s.dateOfBirth || s.birthdate;

        return (
            <tr key={s.id} className="hover:bg-slate-50/50 transition-colors">
                <td className="p-4 font-medium text-slate-700">{s.fullName}</td>
                <td className="p-4 text-slate-500 text-sm">
                    {dateValue ? (
                        new Date(dateValue.split('T')[0]).toLocaleDateString('fr-FR', {
                            year: 'numeric',
                            month: 'long',
                            day: 'numeric'
                        })
                    ) : (
                        // Si vraiment c'est vide, on affiche la valeur brute pour débugger
                        <span className="text-slate-300 italic">
                            {typeof s.birthDate === 'undefined' ? "Champ introuvable" : "Vide"}
                        </span>
                    )}
                </td>
                <td className="p-4 text-right space-x-1">
                    <button 
                        onClick={() => handleOpenEdit(s)}
                        className="p-2 text-slate-400 hover:text-indigo-600 transition-all rounded-lg hover:bg-indigo-50"
                    >
                        <Pencil size={18} />
                    </button>
                    <button 
                        onClick={() => handleDelete(s.id)}
                        className="p-2 text-slate-400 hover:text-rose-600 transition-all rounded-lg hover:bg-rose-50"
                    >
                        <Trash2 size={18} />
                    </button>
                </td>
            </tr>
        );
    }) : (
        <tr>
            <td colSpan="3" className="p-12 text-center text-slate-400 italic text-sm">
                Aucun étudiant trouvé
            </td>
        </tr>
    )}
        </tbody>
                    </table>
                </div>

                {/* Pagination */}
                <div className="flex items-center justify-between mt-6 bg-white p-4 rounded-xl shadow-sm border border-slate-100">
                    <button
                        disabled={pageNumber <= 1}
                        onClick={() => fetchStudents(pageNumber - 1)}
                        className="flex items-center gap-1 text-sm font-medium disabled:opacity-30 hover:text-indigo-600 transition-colors"
                    >
                        <ChevronLeft size={20} /> Précédent
                    </button>
                    <span className="text-slate-500 text-sm font-medium">Page {pageNumber} / {totalPages}</span>
                    <button
                        disabled={pageNumber >= totalPages}
                        onClick={() => fetchStudents(pageNumber + 1)}
                        className="flex items-center gap-1 text-sm font-medium disabled:opacity-30 hover:text-indigo-600 transition-colors"
                    >
                        Suivant <ChevronRight size={20} />
                    </button>
                </div>
            </div>

            {/* MODALE CRÉATION */}
            {isModalOpen && (
                <div className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-3xl w-full max-w-md shadow-2xl animate-in zoom-in duration-200">
                        <div className="p-6 border-b flex justify-between items-center">
                            <h3 className="text-xl font-bold text-slate-800">Nouvel Étudiant</h3>
                            <button onClick={() => setIsModalOpen(false)}><X className="text-slate-400 hover:text-slate-600" /></button>
                        </div>
                        <form onSubmit={handleCreate} className="p-6 space-y-4">
                            <div>
                                <label className="block text-sm font-bold text-slate-700 mb-1">Nom Complet</label>
                                <input 
                                    type="text" required className="w-full border border-slate-200 p-3 rounded-xl outline-indigo-500 transition-all"
                                    placeholder="Ex: Jean Dupont"
                                    onChange={e => setNewStudent({...newStudent, fullName: e.target.value})}
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-bold text-slate-700 mb-1">Date de Naissance</label>
                                
                                <input 
                                    type="date" required className="w-full border border-slate-200 p-3 rounded-xl outline-indigo-500 transition-all"
                                    
                                    onChange={e => setNewStudent({...newStudent, birthDate: e.target.value})}
                                />
                            </div>
                            <button className="w-full bg-indigo-600 text-white py-3 rounded-xl font-bold hover:bg-indigo-700 shadow-lg shadow-indigo-100 transition-all mt-2">
                                Enregistrer
                            </button>
                        </form>
                    </div>
                </div>
            )}

            {/* MODALE BULK ADD */}
            {isBulkModalOpen && (
                <div className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-3xl w-full max-w-xl shadow-2xl animate-in zoom-in duration-200">
                        <div className="p-6 border-b flex justify-between items-center">
                            <h3 className="text-xl font-bold text-slate-800">Ajout Multiple</h3>
                            <button onClick={() => setIsBulkModalOpen(false)}><X className="text-slate-400" /></button>
                        </div>
                        <form onSubmit={handleBulkCreate} className="p-6 space-y-4">
                            <div className="bg-blue-50 p-3 rounded-lg text-xs text-blue-700">
                                <strong>Format:</strong> Nom Complet;YYYY-MM-DD (un par ligne)
                            </div>
                            <textarea 
                                required rows={10} className="w-full border border-slate-200 p-3 rounded-xl outline-indigo-500 font-mono text-sm"
                                placeholder={"Jean Dupont;2000-01-15\nMarie Curie;1998-11-20"}
                                value={bulkInput}
                                onChange={e => setBulkInput(e.target.value)}
                            />
                            <button className="w-full bg-emerald-600 text-white py-3 rounded-xl font-bold hover:bg-emerald-700 shadow-lg shadow-emerald-100 transition-all">
                                Importer les données
                            </button>
                        </form>
                    </div>
                </div>
            )}

            {/* MODALE ÉDITION */}
            {isEditModalOpen && (
                <div className="fixed inset-0 bg-slate-900/40 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                    <div className="bg-white rounded-3xl w-full max-w-md shadow-2xl animate-in zoom-in duration-200">
                        <div className="p-6 border-b flex justify-between items-center">
                            <h3 className="text-xl font-bold text-slate-800">Modifier l'Étudiant</h3>
                            <button onClick={() => setIsEditModalOpen(false)}><X className="text-slate-400 hover:text-slate-600" /></button>
                        </div>
                        <form onSubmit={handleUpdate} className="p-6 space-y-4">
                            <div>
                                <label className="block text-sm font-bold text-slate-700 mb-1">Nom Complet</label>
                                <input 
                                    type="text" required value={editStudent.fullName}
                                    className="w-full border border-slate-200 p-3 rounded-xl outline-indigo-500 transition-all"
                                    onChange={e => setEditStudent({...editStudent, fullName: e.target.value})}
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-bold text-slate-700 mb-1">Date de Naissance</label>
                                <input 
                                    type="date" required value={editStudent.birthDate}
                                    className="w-full border border-slate-200 p-3 rounded-xl outline-indigo-500 transition-all"
                                    onChange={e => setEditStudent({...editStudent, birthDate: e.target.value})}
                                />
                            </div>
                            <button className="w-full bg-indigo-600 text-white py-3 rounded-xl font-bold hover:bg-indigo-700 shadow-lg shadow-indigo-100 transition-all mt-2">
                                Sauvegarder
                            </button>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default StudentList;