import { Link} from "react-router-dom";
import { useState, useEffect } from "react";
import { Project, getProjects, createProject } from "../api/projectclient";
import ProjectCard from "../components/ProjectCard";
import toast from "react-hot-toast";
import Modal from "../components/Modal";

export default function Home() {
  const [projects, setProjects] = useState<Project[]>([]);
  const [loading, setLoading] = useState(true);
  const [newProject, setNewProject] = useState({ name: "", description: "" });
  const [isModalOpen, setIsModalOpen] = useState(false);

  const handleOpenModal = () => setIsModalOpen(true);
  const handleCloseModal = () => setIsModalOpen(false);

  useEffect(() => {
    const fetchProjects = async () => {
      try {
        const data = await getProjects();
        setProjects(data);
      } catch (error) {
        toast.error("Failed to fetch projects");
        console.error("Fetch error:", error);
      } finally {
        setLoading(false);
      }
    };

    fetchProjects();
  }, []);

  const handleAddNewProject = async () => {
    const resp = await createProject(newProject.name, newProject.description);
    setProjects([...projects, resp]);
    toast.success(`Project ${newProject.name} created`);
    setIsModalOpen(false);
  }

  return (
    <div className="flex h-screen justify-center">
      <div className="text-center">
        <h1 className="text-4xl font-bold">This is where we put the main content</h1>
        {loading && <p>Loading projects...</p>}

        <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-6 gap-3">
        {projects.map((project) => (
          <ProjectCard key={project.id} project={project} />
        ))}
        <ProjectCard onAddNew={handleOpenModal} />
        </div>

        <Modal isOpen={isModalOpen} onClose={handleCloseModal}>
          <h2 className="text-2xl font-bold mb-4">Create New Project</h2>
          <input
            type="text"
            placeholder="Project Name"
            value={newProject.name}
            onChange={(e) => setNewProject({ ...newProject, name: e.target.value })}
            className="w-full p-2 border rounded-lg mb-4"
          />
          <textarea
            placeholder="Description"
            value={newProject.description}
            onChange={(e) => setNewProject({ ...newProject, description: e.target.value })}
            className="w-full p-2 border rounded-lg mb-4"
          />
          <button
            onClick={handleAddNewProject}
            className="bg-blue-500 text-white px-4 py-2 rounded-lg"
          > Create Project
          </button>
        </Modal>

        <div className="space-x-4">
          <Link to="/login" className="text-blue-500 underline">
            Log out
          </Link>
          <br />
        </div>
      </div>
    </div>
  );
}
