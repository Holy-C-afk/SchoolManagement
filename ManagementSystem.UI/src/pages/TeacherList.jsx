import React, { useEffect, useState } from 'react';
import api from '../api/api';
import { 
    Users, Trash2, Plus, X, Pencil, Search, 
    ChevronLeft, ChevronRight, FileText, RefreshCw 
} from 'lucide-react';

const TeacherList = () => {
    // États pour les données
    const [teachers, setTeachers] = useState([]);
    const [departments, setDepartments] = useState([]);
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

    // États Formulaires
    const [newTeacher, setNewTeacher] = useState({ fullName: '', departmentId: '' });
    const [editTeacher, setEditTeacher] = useState({ id: null, fullName: '', departmentId: '' });

    // 1. Charger les enseignants et les départements
    const fetchData = async (page = pageNumber) => {
        try {
            setLoading(true);
            const [teacherRes, deptRes] = await Promise.all([
                api.get('/Teachers/paged', {
                    params: { pageNumber: page, pageSize, search }
                }),
                api.get('/Departments') // Pour remplir le select dans les modales
            ]);
            
            setTeachers(teacherRes.data.items);
            setTotalPages(teacherRes.data.totalPages);
            setPageNumber(teacherRes.data.pageNumber);
            setDepartments(deptRes.data);
            setError(null);
        } catch (err) {
            setError("Erreur de communication avec l'API");
            console.error(err);
        } finally {
            setLoading(false);
        }
    };

    useEffect(() => {
        fetchData(1);
    }, [search]);

    // 2. Actions (Create, Update, Delete)
    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            await api.post('/Teachers', newTeacher);
            setIsModalOpen(false);
            setNewTeacher({ fullName: '', departmentId: '' });
            fetchData();
        } catch (err) {
            alert("Erreur lors de la création : " + (err.response?.data || err.message));
        }
    };

    const handleUpdate = async (e) => {
        e.preventDefault();
        try {
            await api.put(`/Teachers/${editTeacher.id}`, {
                fullName: editTeacher.fullName,
                departmentId: editTeacher.departmentId
            });
            setIsEditModalOpen(false);
            fetchData();
        } catch (err) {
            alert("Erreur lors de la mise à jour");
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Supprimer cet enseignant ?")) return;
        try {
            await api.delete(`/Teachers/${id}`);
            fetchData();
        } catch (err) {
            alert("Erreur lors de la suppression");
        }
    };

    const handleExportPdf = async () => {
        try {
            const res = await api.get('/Teachers/export/pdf', { 
                params: { search },
                responseType: 'blob' 
            });
            const url = window.URL.createObjectURL(new Blob([res.data]));
            const link = document.createElement('a');
            link.href = url;
            link.setAttribute('download', `Enseignants_${new Date().toISOString().split('T')[0]}.pdf`);
            document.body.appendChild(link);
            link.click();
            link.remove();
        } catch (err) {
            alert("Erreur export PDF");
        }
    };

    if (loading && teachers.length === 0) return <div className="p-10 text-center animate-pulse">Chargement des enseignants...</div>;

    return (
        <div className="min-h-screen bg-slate-50 p-4 md:p-8">
            <div className="max-w-6xl mx-auto">
                
                {/* Header */}
                <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-8">
                    <div>
                        <h1 className="text-3xl font-bold text-slate-900 flex items-center gap-3">
                            <Users className="text-indigo-600 size-10" />
                            Enseignants
                        </h1>
                    </div>
                    
                    <div className="flex flex-col md:flex-row gap-3 items-stretch md:items-center">
                        <div className="relative">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 size-4" />
                            <input
                                type="text"
                                placeholder="Rechercher un prof..."
                                className="w-full md:w-64 pl-9 pr-3 py-2 rounded-xl border border-slate-200 outline-none focus:ring-2 focus:ring-indigo-500"
                                value={search}
                                onChange={e => setSearch(e.target.value)}
                            />
                        </div>
                        <button 
                            onClick={() => setIsModalOpen(true)}
                            className="bg-indigo-600 text-white px-5 py-2 rounded-xl font-semibold hover:bg-indigo-700 flex items-center gap-2 shadow-lg shadow-indigo-100"
                        >
                            <Plus size={20} /> Nouveau
                        </button>
                        <button 
                            onClick={handleExportPdf}
                            className="bg-slate-800 text-white px-4 py-2 rounded-xl hover:bg-slate-900 flex items-center gap-2"
                        >
                            <FileText size={18} /> PDF
                        </button>
                    </div>
                </div>

                {/* Table / List */}
                <div className="bg-white rounded-2xl shadow-sm border border-slate-100 overflow-hidden">
                    <table className="w-full text-left border-collapse">
                        <thead className="bg-slate-50 border-b border-slate-100">
                            <tr>
                                <th className="p-4 font-semibold text-slate-600">Nom Complet</th>
                                <th className="p-4 font-semibold text-slate-600">Département</th>
                                <th className="p-4 text-right">Actions</th>
                            </tr>
                        </thead>
                        <tbody className="divide-y divide-slate-100">
                            {teachers.length > 0 ? teachers.map(t => (
                                <tr key={t.id} className="hover:bg-slate-50/50 transition-colors">
                                    <td className="p-4 font-medium text-slate-700">{t.fullName}</td>
                                    <td className="p-4">
                                        <span className="px-3 py-1 bg-indigo-50 text-indigo-700 rounded-full text-xs font-bold uppercase tracking-wider">
                                            {t.departmentName || "Sans département"}
                                        </span>
                                    </td>
                                    <td className="p-4 text-right space-x-2">
                                        <button 
                                            onClick={() => {
                                                setEditTeacher({ id: t.id, fullName: t.fullName, departmentId: t.departmentId });
                                                setIsEditModalOpen(true);
                                            }}
                                            className="p-2 text-slate-400 hover:text-blue-600 hover:bg-blue-50 rounded-lg transition-all"
                                        >
                                            <Pencil size={18} />
                                        </button>
                                        <button 
                                            onClick={() => handleDelete(t.id)}
                                            className="p-2 text-slate-400 hover:text-rose-600 hover:bg-rose-50 rounded-lg transition-all"
                                        >
                                            <Trash2 size={18} />
                                        </button>
                                    </td>
                                </tr>
                            )) : (
                                <tr>
                                    <td colSpan="3" className="p-12 text-center text-slate-400 italic">Aucun enseignant trouvé</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>

                {/* Pagination */}
                <div className="flex items-center justify-between mt-6 bg-white p-4 rounded-xl shadow-sm border border-slate-100">
                    <button
                        disabled={pageNumber <= 1}
                        onClick={() => fetchData(pageNumber - 1)}
                        className="flex items-center gap-1 text-sm font-medium disabled:opacity-30 hover:text-indigo-600 transition-colors"
                    >
                        <ChevronLeft size={20} /> Précédent
                    </button>
                    <span className="text-slate-500 text-sm font-medium">Page {pageNumber} / {totalPages}</span>
                    <button
                        disabled={pageNumber >= totalPages}
                        onClick={() => fetchData(pageNumber + 1)}
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
                            <h3 className="text-xl font-bold">Nouvel Enseignant</h3>
                            <button onClick={() => setIsModalOpen(false)}><X className="text-slate-400" /></button>
                        </div>
                        <form onSubmit={handleCreate} className="p-6 space-y-4">
                            <div>
                                <label className="block text-sm font-bold mb-1">Nom Complet</label>
                                <input 
                                    type="text" required className="w-full border p-3 rounded-xl outline-indigo-500"
                                    onChange={e => setNewTeacher({...newTeacher, fullName: e.target.value})}
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-bold mb-1">Département</label>
                                <select 
                                    required className="w-full border p-3 rounded-xl outline-indigo-500 bg-white"
                                    onChange={e => setNewTeacher({...newTeacher, departmentId: e.target.value})}
                                >
                                    <option value="">Sélectionnez...</option>
                                    {departments.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
                                </select>
                            </div>
                            <button className="w-full bg-indigo-600 text-white py-3 rounded-xl font-bold hover:bg-indigo-700 shadow-lg shadow-indigo-100">
                                Enregistrer
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
                            <h3 className="text-xl font-bold">Modifier l'Enseignant</h3>
                            <button onClick={() => setIsEditModalOpen(false)}><X className="text-slate-400" /></button>
                        </div>
                        <form onSubmit={handleUpdate} className="p-6 space-y-4">
                            <div>
                                <label className="block text-sm font-bold mb-1">Nom Complet</label>
                                <input 
                                    type="text" required value={editTeacher.fullName}
                                    className="w-full border p-3 rounded-xl outline-indigo-500"
                                    onChange={e => setEditTeacher({...editTeacher, fullName: e.target.value})}
                                />
                            </div>
                            <div>
                                <label className="block text-sm font-bold mb-1">Département</label>
                                <select 
                                    required value={editTeacher.departmentId}
                                    className="w-full border p-3 rounded-xl outline-indigo-500 bg-white"
                                    onChange={e => setEditTeacher({...editTeacher, departmentId: e.target.value})}
                                >
                                    <option value="">Sélectionnez...</option>
                                    {departments.map(d => <option key={d.id} value={d.id}>{d.name}</option>)}
                                </select>
                            </div>
                            <button className="w-full bg-blue-600 text-white py-3 rounded-xl font-bold hover:bg-blue-700 shadow-lg shadow-blue-100">
                                Sauvegarder les modifications
                            </button>
                        </form>
                    </div>
                </div>
            )}
        </div>
    );
};

export default TeacherList;