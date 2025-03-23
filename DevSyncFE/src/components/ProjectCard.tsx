import { useNavigate } from "react-router-dom";
import { Project } from "../api/projectclient";

interface ProjectCardProps {
  project?: Project;
  onAddNew?: () => void;
}

export default function ProjectCard({ project, onAddNew }: ProjectCardProps) {
  const navigate = useNavigate();

  const handleClick = () => {
    if (project) {
      navigate(`/projects/${project.id}`);
    } else if (onAddNew) {
      onAddNew();
    }
  };

  return (
    <div
      onClick={handleClick}
      className="bg-white p-6 rounded-2xl shadow-lg hover:shadow-xl
      transition-transform transform hover:scale-105 cursor-pointer border-2
      border-gray-300 hover:border-blue-500 max-w-sm flex items-center justify-center"
    >
      {project ? (
        <div>
          <h2 className="text-2xl font-semibold mb-2">{project.name}</h2>
          <p className="text-gray-600">{project.description}</p>
        </div>
      ) : (
        <span className="text-xl font-semibold">+ Add Project</span>
      )}
    </div>
  );
}
