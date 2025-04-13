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
            const response = await fetch("http://localhost:5000/api/v0/classes/academic-state", {
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
                    name: materia.name,
                    state: materia.state,
                    grade: materia.grade,
                    classId: materia.classId,
                    plan: materia.plan
                }));
                materias.forEach(materia => {
                    if(materia.state == "PASSED") {
                        const fila = document.createElement('tr');
                        fila.innerHTML = `
                            <td>${materia.year}</td>
                            <td>${materia.name}</td>
                            <td>Aprobada con ${materia.grade} en la comision ${materia.classId}</td>
                            <td>${materia.plan}</td>
                        `;
                        document.getElementById("state-table").appendChild(fila);
                    } else if(materia.state == "ONCOURSE") {
                        const fila = document.createElement('tr');
                        fila.innerHTML = `
                            <td>${materia.year}</td>
                            <td>${materia.name}</td>
                            <td>Cursa en: ${materia.classId}</td>
                            <td>${materia.plan}</td>
                        `;
                        document.getElementById("state-table").appendChild(fila);
                    } else if(materia.state == "FINAL") {
                        const fila = document.createElement('tr');
                        fila.innerHTML = `
                            <td>${materia.year}</td>
                            <td>${materia.name}</td>
                            <td>Rinde con: ${materia.classId}</td>
                            <td>${materia.plan}</td>
                        `;
                        document.getElementById("state-table").appendChild(fila);
                    }
                    else{
                        const fila = document.createElement('tr');
                        fila.innerHTML = `
                            <td>${materia.year}</td>
                            <td>${materia.name}</td>
                            <td></td>
                            <td>${materia.plan}</td>
                        `;
                        document.getElementById("state-table").appendChild(fila);   
                    }
                    });
            } else {
                const errorData = await response.text();
                alert(`Error: ${errorData}`);
            }
        } catch (error) {
            alert("Error during state fetch.");
        }
    } catch (error) {
        console.error('Error al cargar materias:', error);
    }
}

cargarMaterias();