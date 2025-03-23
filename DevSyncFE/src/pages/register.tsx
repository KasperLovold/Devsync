import { Link } from "react-router-dom";

export default function Register() {
  return (
    <div className="flex h-screen items-center justify-center">
      <div className="p-8 max-w-md w-full space-y-6 bg-white shadow-xl rounded-2xl">
        <h1 className="text-2xl font-bold text-center">Registrer deg</h1>

        <form className="space-y-4">
          <input
            type="text"
            placeholder="Navn"
            className="w-full p-3 border rounded-lg"
          />
          <input
            type="email"
            placeholder="E-post"
            className="w-full p-3 border rounded-lg"
          />
          <input
            type="password"
            placeholder="Passord"
            className="w-full p-3 border rounded-lg"
          />
          <button
            type="submit"
            className="w-full bg-green-500 text-white p-3 rounded-lg hover:bg-green-600"
          >
            Opprett konto
          </button>
        </form>

        <p className="text-center">
          Har du allerede en konto?{" "}
          <Link to="/login" className="text-green-500 hover:underline">
            Logg inn
          </Link>
        </p>
      </div>
    </div>
  );
}
