import { verifyToken } from "./verifyToken.js";

async function cargarMaterias() {
    const token = document.cookie.replace(/(?:(?:^|.*;\s*)token\s*=\s*([^;]*).*$)|^.*$/, "$1");
    
    if (!token) {
        alert("Token not found, please re login.");
        window.location.href = "login.html";
    } else if (!(await verifyToken())) {
        alert("Token expired, please re login.");
        window.location.href = "login.html";
    }

    try {
        try {
            const response = await fetch("http://localhost:5000/api/v0/classes", {
                method: "GET",
                headers: {
                    "Content-Type": "application/json",
                    "Authorization": `Bearer ${token}`
                },
            });
    
            if (response.ok) {
                const result = await response.json();
                const materias = result.subjects.map(materia => ({
                    year: materia.year,
                    duration: materia.duration,
                    name: materia.name
                }));
                materias.forEach(materia => {
                    const fila = document.createElement('tr');
                    fila.innerHTML = `
                        <td>${materia.year}</td>
                        <td>${materia.duration}</td>
                        <td>${materia.name}</td>
                    `;
                    document.getElementById("subjects-table").appendChild(fila);
                    });
            } else {
                const errorData = await response.text();
                alert(`Error: ${errorData}`);
            }
        } catch (error) {
            alert("Error during subjects fetch.");
        }
    } catch (error) {
        console.error('Error al cargar materias:', error);
    }
}

cargarMaterias();