const res = await api.get('/Teachers/paged', {
    params: { pageNumber: page, pageSize, search: search }
});
const response = await api.get('/Teachers/export/pdf', {
    params: { search: search },
    responseType: 'blob'
});