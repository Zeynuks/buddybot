import type { ReactNode } from "react";
import { createContext, useState, useEffect } from "react";
import { AuthenticationService } from "../services/authService";
import type { UserLookup } from "../models/User";
import { getCurrentUser } from "../services/userService";
import { parsePermissionsFromToken } from "../utils/parsePermissionsFromToken";

type AuthContextType = {
    isAuthenticated: boolean;
    loading: boolean;
    user: UserLookup | null;
    permissions: string[];
    login: (login: string, password: string) => Promise<boolean>;
    logout: () => void;
};

const AuthContext = createContext<AuthContextType>({
    isAuthenticated: false,
    loading: true,
    user: null,
    permissions: [],
    login: async () => false,
    logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
    const [isAuthenticated, setIsAuthenticated] = useState(false);
    const [loading, setLoading] = useState(true);
    const [user, setUser] = useState<UserLookup | null>(null);
    const [permissions, setPermissions] = useState<string[]>([]);

    useEffect(() => {
        const checkAuth = async () => {
            const authService = new AuthenticationService();
            const accessToken = authService.getAccessToken();
            if (accessToken) {
                setIsAuthenticated(true);
                setPermissions(parsePermissionsFromToken(accessToken));
                try {
                    const userData = await getCurrentUser();
                    setUser(userData);
                } catch {
                    setUser(null);
                }
                setLoading(false);
            } else if (authService.getRefreshToken()) {
                const result = await authService.refreshToken();
                if (result.accessToken) {
                    setIsAuthenticated(true);
                    setPermissions(parsePermissionsFromToken(result.accessToken));
                    try {
                        const userData = await getCurrentUser();
                        setUser(userData);
                    } catch {
                        setUser(null);
                    }
                } else {
                    authService.logout();
                    setIsAuthenticated(false);
                    setUser(null);
                    setPermissions([]);
                }
                setLoading(false);
            } else {
                setIsAuthenticated(false);
                setUser(null);
                setPermissions([]);
                setLoading(false);
            }
        };
        checkAuth();
    }, []);

    const login = async (login: string, password: string) => {
        const authService = new AuthenticationService();
        const result = await authService.authentication({ Login: login, Password: password });
        if (result.accessToken) {
            setIsAuthenticated(true);
            setPermissions(parsePermissionsFromToken(result.accessToken));
            try {
                const userData = await getCurrentUser();
                setUser(userData);
            } catch {
                setUser(null);
            }
            return true;
        }
        setIsAuthenticated(false);
        setUser(null);
        setPermissions([]);
        return false;
    };

    const logout = () => {
        const authService = new AuthenticationService();
        authService.logout();
        setIsAuthenticated(false);
        setUser(null);
        setPermissions([]);
    };

    return (
        <AuthContext.Provider value={{ isAuthenticated, loading, user, permissions, login, logout }}>
            {children}
        </AuthContext.Provider>
    );
};

export default AuthContext;
