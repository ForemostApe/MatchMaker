import { useNavigate } from "react-router-dom";
import LoginForm from "../../components/LoginForm/LoginForm";
import { AuthService } from "../../services/authService";

const LoginPage = () => {
    const navigate = useNavigate();

    const handleLogin = async (credentials) => {
        try {
            await AuthService.login(credentials);
            navigate('/home');
        } catch (error) {
            alert('Login failed.')
        }
    };

    return (
        <div className="min-h-screen flex items-center justify-center">
            <LoginForm onSubmit={handleLogin} />
        </div>
    );
};

export default LoginPage;