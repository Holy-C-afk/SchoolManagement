import { useEffect, useState } from 'react';
import api from '../api/api';
import { GraduationCap, Trash2, Plus, X, Pencil, Search } from 'lucide-react';

const StudentList = ({ onLogout }) => {
    const [students, setStudents] = useState([]);
    const [pageNumber, setPageNumber] = useState(1);
    const [pageSize] = useState(10);
    const [totalPages, setTotalPages] = useState(1);
    const [fetchError, setFetchError] = useState('');
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [isBulkModalOpen, setIsBulkModalOpen] = useState(false);
    const [isEditModalOpen, setIsEditModalOpen] = useState(false);
    const [newStudent, setNewStudent] = useState({
        fullName: '',
        birthDate: '' 
    });
    const [editStudent, setEditStudent] = useState({
        id: null,
        fullName: '',
        birthDate: ''
    });
    const [bulkInput, setBulkInput] = useState('');
    const [search, setSearch] = useState('');


    // Charger les étudiants
    const fetchStudents = async (page = pageNumber) => {
    try {
        const res = await api.get('/Students/paged', {
            params: { 
                pageNumber: page, 
                pageSize: pageSize,
                search: search // On utilise le paramètre passé à la fonction
            }
        });
        setStudents(res.data.items);
        setPageNumber(res.data.pageNumber);
        setTotalPages(res.data.totalPages);
    } catch (err) {
        const status = err?.response?.status;
        const data = err?.response?.data;
        const msg =
            status
                ? `Erreur API (${status}) sur GET /Students: ${typeof data === 'string' ? data : JSON.stringify(data)}`
                : `Erreur réseau sur GET /Students: ${err.message}`;

        console.error("Erreur fetchStudents:", err.response || err.message);
        setFetchError(msg);
        setStudents([]);
    }
};


    useEffect(() => {
    // On passe explicitement 'search' ici
    fetchStudents(1, search); 
}, [search]);

    // Fonction de création
    const handleCreate = async (e) => {
        e.preventDefault();
        try {
            const names = newStudent.fullName.split(' ');
            await api.post('/Students', {
                firstName: names[0] || '',
                lastName: names.slice(1).join(' ') || '',
                birthDate: newStudent.birthDate
            });
            setIsModalOpen(false);
            setNewStudent({ fullName: '', birthDate: '' });
            fetchStudents();
        } catch (err) {
            console.error("Erreur création étudiant :", err.response?.data || err.message);
            alert(
                err.response?.data?.message ||
                JSON.stringify(err.response?.data) ||
                "Erreur lors de la création"
            );
        }
    };

    const handleDelete = async (id) => {
        if (!window.confirm("Supprimer cet étudiant ?")) return;
        try {
            await api.delete(`/Students/${id}`);
            fetchStudents();
        } catch (err) {
            console.error("Erreur suppression étudiant :", err.response?.data || err.message);
            alert("Erreur lors de la suppression");
        }
    };

    const handleBulkCreate = async (e) => {
        e.preventDefault();
        try {
            const lines = bulkInput
                .split('\n')
                .map(l => l.trim())
                .filter(Boolean);

            const payload = lines.map(line => {
                const [namePart, datePart] = line.split(';').map(p => p.trim());
                if (!namePart || !datePart) {
                    throw new Error("Format invalide. Utilisez: Nom Complet;YYYY-MM-DD");
                }
                const names = namePart.split(' ');
                return {
                    firstName: names[0] || '',
                    lastName: names.slice(1).join(' ') || '',
                    birthDate: datePart
                };
            });

            await api.post('/Students/bulk', payload);
            setIsBulkModalOpen(false);
            setBulkInput('');
            fetchStudents();
        } catch (err) {
            console.error("Erreur ajout multiple :", err.response?.data || err.message);
            alert(
                err.message ||
                err.response?.data?.message ||
                "Erreur lors de l'ajout multiple"
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
    const handleExportPdf = async () => {
        try {
            const response = await api.get('/Students/export/pdf', {
            // C'est ici qu'on envoie le filtre actuel au backend
            params: { search: search }, 
            responseType: 'blob', 
        });

            const url = window.URL.createObjectURL(new Blob([response.data], { type: 'application/pdf' }));
            const link = document.createElement('a');
            link.href = url;
            
            
            const fileName = search ? `Etudiants_Filtre_${search}.pdf` : "Liste_Etudiants.pdf";
            link.setAttribute('download', fileName);
            
            document.body.appendChild(link);
            link.click();
            link.parentNode.removeChild(link);
            window.URL.revokeObjectURL(url);
        } catch (err) {
            console.error("Erreur export PDF:", err);
            alert("Erreur lors de la génération du PDF (vérifiez vos droits d'accès)");
        }
    };

    /*const filteredStudents = students.filter(student =>
        student.fullName?.toLowerCase().includes(search.toLowerCase())
    );*/

    return (
        <div className="min-h-screen bg-slate-50 p-4 md:p-8">
            <div className="max-w-6xl mx-auto">
                
                {/* Header avec bouton Ajouter + recherche */}
                <div className="flex flex-col md:flex-row md:items-center md:justify-between gap-4 mb-8">
                    <div>
                        <h1 className="text-3xl font-bold text-slate-900 flex items-center gap-3">
                            <GraduationCap className="text-blue-600 size-10" />
                            Étudiants
                        </h1>
                        <p className="text-slate-500 mt-1 text-sm">
                            Gérez les étudiants : ajout, édition, suppression et filtrage par nom.
                        </p>
                    </div>
                    <div className="flex flex-col md:flex-row gap-3 items-stretch md:items-center">
                        <div className="relative">
                            <Search className="absolute left-3 top-1/2 -translate-y-1/2 text-slate-400 size-4" />
                            <input
                                type="text"
                                placeholder="Rechercher par nom..."
                                className="w-full md:w-64 pl-9 pr-3 py-2.5 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none text-sm"
                                value={search}
                                onChange={e => setSearch(e.target.value)}
                            />
                        </div>
                        <div className="flex gap-3">
                            <button 
                                onClick={() => setIsModalOpen(true)}
                                className="bg-blue-600 text-white px-5 py-2.5 rounded-xl font-semibold hover:bg-blue-700 transition-all flex items-center gap-2 shadow-lg shadow-blue-200"
                            >
                                <Plus className="size-5" /> Nouvel Étudiant
                            </button>
                            <button 
                                onClick={() => setIsBulkModalOpen(true)}
                                className="bg-emerald-600 text-white px-5 py-2.5 rounded-xl font-semibold hover:bg-emerald-700 transition-all flex items-center gap-2 shadow-lg shadow-emerald-200"
                            >
                                <Plus className="size-5" /> Ajout multiple
                            </button>
                        </div>
                        <button onClick={onLogout} className="text-slate-500 hover:text-rose-600 px-4 transition-colors whitespace-nowrap">
                            Déconnexion
                        </button>
                    </div>
                </div>

                {/* Liste filtrée des étudiants */}
                {fetchError && (
                    <div className="p-4 rounded-xl border border-rose-200 bg-rose-50 text-rose-700 text-sm mb-4">
                        <div className="font-semibold">Impossible de charger les étudiants</div>
                        <div className="mt-1 break-words">{fetchError}</div>
                        <div className="mt-3 flex gap-2">
                            <button
                                onClick={fetchStudents}
                                className="px-3 py-2 rounded-lg bg-rose-600 text-white text-sm font-semibold hover:bg-rose-700"
                            >
                                Réessayer
                            </button>
                        </div>
                    </div>
                )}
                <div className="flex items-center justify-between mt-6">
                <button
                    disabled={pageNumber <= 1}
                    onClick={() => fetchStudents(pageNumber - 1)}
                    className="px-3 py-2 rounded-lg border text-sm disabled:opacity-50"
                >
                    Précédent
                </button>

                <span className="text-sm text-slate-600">
                    Page {pageNumber} / {totalPages}
                </span>

                <button
                    disabled={pageNumber >= totalPages}
                    onClick={() => fetchStudents(pageNumber + 1)}
                    className="px-3 py-2 rounded-lg border text-sm disabled:opacity-50"
                >
                    Suivant
                </button>
                </div>
                <div className="space-y-4">
                    {students.length > 0 ? (
                        students.map(student => (
                            <div key={student.id} className="p-4 border rounded-xl shadow-sm flex justify-between items-center bg-white">
                                <div>
                                    <div className="font-bold text-slate-800">{student.fullName}</div>
                                    <div className="text-sm text-slate-500">{student.birthDate}</div>
                                </div>
                                <div className="flex items-center gap-2">
                                    <button
                                        onClick={() => handleOpenEdit(student)}
                                        className="text-blue-500 hover:text-blue-700 p-2 rounded-full hover:bg-blue-50 transition-colors"
                                        title="Modifier"
                                    >
                                        <Pencil className="size-5" />
                                    </button>
                                    <button
                                        onClick={() => handleDelete(student.id)}
                                        className="text-rose-500 hover:text-rose-700 p-2 rounded-full hover:bg-rose-50 transition-colors"
                                        title="Supprimer"
                                    >
                                        <Trash2 className="size-5" />
                                    </button>
                                </div>
                            </div>  
                        ))
                    ) : (
                        <div className="text-slate-500 text-sm">
                            Aucun étudiant trouvé{search ? " pour ce filtre" : ""}.
                        </div>
                    )}
                </div>


                {/* MODALE DE CRÉATION (S'affiche si isModalOpen est true) */}
                {isModalOpen && (
                    <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-3xl shadow-2xl w-full max-w-md overflow-hidden animate-in fade-in zoom-in duration-200">
                            <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                <h3 className="text-xl font-bold text-slate-800">Ajouter un étudiant</h3>
                                <button onClick={() => setIsModalOpen(false)} className="text-slate-400 hover:text-slate-600 transition-colors">
                                    <X className="size-6" />
                                </button>
                            </div>
                            
                            <form onSubmit={handleCreate} className="p-6 space-y-4">
                                <div>
                                    <label className="block text-sm font-semibold text-slate-700 mb-1">Nom Complet</label>
                                    <input 
                                        type="text" 
                                        required
                                        className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                        placeholder="Ex: Tachfine EL Farouki"
                                        onChange={e => setNewStudent({...newStudent, fullName: e.target.value})}
                                    />
                                </div>
                                <div>
                                    <label className="block text-sm font-semibold text-slate-700 mb-1">Date de Naissance</label>
                                    <input 
                                        type="date" 
                                        required
                                        className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                        onChange={e => setNewStudent({...newStudent, birthDate: e.target.value})}
                                    />
                                </div>
                                

                                <button 
                                    type="submit"
                                    className="w-full bg-blue-600 text-white py-3 rounded-xl font-bold mt-4 hover:bg-blue-700 transition-all shadow-lg shadow-blue-100"
                                >
                                    Enregistrer l'étudiant
                                </button>
                            </form>
                        </div>
                    </div>
                )}

                {/* MODALE D'AJOUT MULTIPLE */}
                {isBulkModalOpen && (
                    <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-3xl shadow-2xl w-full max-w-xl overflow-hidden animate-in fade-in zoom-in duration-200">
                            <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                <h3 className="text-xl font-bold text-slate-800">Ajout multiple d'étudiants</h3>
                                <button onClick={() => setIsBulkModalOpen(false)} className="text-slate-400 hover:text-slate-600 transition-colors">
                                    <X className="size-6" />
                                </button>
                            </div>
                            
                            <form onSubmit={handleBulkCreate} className="p-6 space-y-4">
                                <p className="text-sm text-slate-600">
                                    Saisissez un étudiant par ligne, au format&nbsp;:
                                    <span className="font-mono bg-slate-100 px-2 py-1 rounded-md ml-1">
                                        Nom Complet;YYYY-MM-DD
                                    </span>
                                </p>
                                <textarea
                                    rows={8}
                                    className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-emerald-500 outline-none transition-all font-mono text-sm"
                                    placeholder={"Ex:\nTachfine EL Farouki;2000-05-12\nJohn Doe;1999-10-01"}
                                    value={bulkInput}
                                    onChange={e => setBulkInput(e.target.value)}
                                />

                                <button 
                                    type="submit"
                                    className="w-full bg-emerald-600 text-white py-3 rounded-xl font-bold mt-2 hover:bg-emerald-700 transition-all shadow-lg shadow-emerald-100"
                                >
                                    Enregistrer les étudiants
                                </button>
                            </form>
                        </div>
                    </div>
                )}

                {/* MODALE D'ÉDITION */}
                {isEditModalOpen && (
                    <div className="fixed inset-0 bg-slate-900/50 backdrop-blur-sm flex items-center justify-center z-50 p-4">
                        <div className="bg-white rounded-3xl shadow-2xl w-full max-w-md overflow-hidden animate-in fade-in zoom-in duration-200">
                            <div className="p-6 border-b border-slate-100 flex justify-between items-center">
                                <h3 className="text-xl font-bold text-slate-800">Modifier l'étudiant</h3>
                                <button onClick={() => setIsEditModalOpen(false)} className="text-slate-400 hover:text-slate-600 transition-colors">
                                    <X className="size-6" />
                                </button>
                            </div>
                            
                            <form onSubmit={handleUpdate} className="p-6 space-y-4">
                                <div>
                                    <label className="block text-sm font-semibold text-slate-700 mb-1">Nom Complet</label>
                                    <input 
                                        type="text" 
                                        required
                                        className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                        placeholder="Ex: Tachfine EL Farouki"
                                        value={editStudent.fullName}
                                        onChange={e => setEditStudent({...editStudent, fullName: e.target.value})}
                                    />
                                </div>
                                <div>
                                    <label className="block text-sm font-semibold text-slate-700 mb-1">Date de Naissance</label>
                                    <input 
                                        type="date" 
                                        required
                                        className="w-full px-4 py-3 rounded-xl border border-slate-200 focus:ring-2 focus:ring-blue-500 outline-none transition-all"
                                        value={editStudent.birthDate}
                                        onChange={e => setEditStudent({...editStudent, birthDate: e.target.value})}
                                    />
                                </div>

                                <button 
                                    type="submit"
                                    className="w-full bg-blue-600 text-white py-3 rounded-xl font-bold mt-4 hover:bg-blue-700 transition-all shadow-lg shadow-blue-100"
                                >
                                    Enregistrer les modifications
                                </button>
                            </form>
                        </div>
                    </div>
                )}
            </div>
            <button
                onClick={handleExportPdf}
                className="bg-indigo-600 text-white px-5 py-2.5 rounded-xl font-semibold hover:bg-indigo-700 transition-all flex items-center gap-2"
                >
                Export PDF
                </button>

        </div>
        
    );
};
export default StudentList;
