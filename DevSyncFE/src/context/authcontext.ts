import {
  createContext,
  ReactNode,
  useState,
  useEffect,
  useCallback,
  createElement,
} from "react";

type Props = {
  children?: ReactNode;
};

type IAuthContext = {
  isAuthenticated: boolean;
  token: string | null;
  login: (token: string) => void;
  logout: () => void;
};

const InitialState: IAuthContext = {
  isAuthenticated: false,
  token: null,
  login: () => {},
  logout: () => {},
};

export const AuthContext = createContext<IAuthContext>(InitialState);

export const AuthProvider = ({ children }: Props) => {
  const [isAuthenticated, setIsAuthenticated] = useState(false);
  const [token, setToken] = useState<string | null>(null);

  const logout = useCallback(() => {
    localStorage.removeItem("token");
    sessionStorage.clear();
    setToken(null);
    setIsAuthenticated(false);
  }, []);

  const login = useCallback((token: string) => {
    localStorage.setItem("token", token);
    setToken(token);
    setIsAuthenticated(true);
  }, []);

  useEffect(() => {
    const storedToken = localStorage.getItem("token");
    if (storedToken) {
      try {
        setToken(storedToken);
        setIsAuthenticated(true);
      } catch {
        logout();
      }
    }
  }, [logout]);

  return createElement(
    AuthContext.Provider,
    { value: { token, login, logout, isAuthenticated } },
    children,
  );
};
