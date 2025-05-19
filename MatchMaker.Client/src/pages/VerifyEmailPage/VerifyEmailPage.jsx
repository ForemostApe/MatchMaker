import { useEffect, useState } from "react";
import { useSearchParams, useNavigate } from "react-router-dom";
import api from "../../services/axiosConfig";

const VerifyEmailPage = () => {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const [status, setStatus] = useState("loading");

  useEffect(() => {
    const verificationToken = searchParams.get("verificationToken");

    const verifyEmail = async () => {
      try {
        if (!verificationToken) {
          setStatus("error");
          return;
        }

        await api.put(`/Auth/verify-email?verificationToken=${verificationToken}`);
        setStatus("success");
      } catch (err) {
        console.error("Verification failed:", err);
        setStatus("error");
      }
    };

    verifyEmail();
  }, [searchParams]);

  useEffect(() => {
    if (status === "success" || status === "error") {
      const timer = setTimeout(() => {
        navigate("/");
      }, 3000);

      return () => clearTimeout(timer);
    }
  }, [status, navigate]);

  return (
    <div className="min-h-screen flex items-center justify-center">
      {status === "loading" && <p>Verifierar email-adress...</p>}
      {status === "success" && <p className="text-green-600 font-bold">Din email-adress är verifierad. Du omdirigeras till startsidan.</p>}
      {status === "error" && <p className="text-red-600 fond-bold">Din email-adress kunde inte verifieras. Du omdirigeras till startsidan.</p>}
    </div>
  );
};

export default VerifyEmailPage;
