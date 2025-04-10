async function cargarMaterias() {
    try {
        const response = await fetch('/api/subjects');
        const materias = await response.json();

        const tbody = document.getElementById('materias-body');
        materias.forEach(materia => {
        const fila = document.createElement('tr');
        fila.innerHTML = `
            <td>${materia.anio}</td>
            <td>${materia.cuatrimestre}</td>
            <td>${materia.nombre}</td>
        `;
        tbody.appendChild(fila);
        });
    } catch (error) {
        console.error('Error al cargar materias:', error);
    }
}

cargarMaterias();