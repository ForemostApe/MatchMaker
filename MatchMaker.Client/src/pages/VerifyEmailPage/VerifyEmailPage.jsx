import { useEffect, useState } from "react";
import { useSearchParams } from "react-router-dom";
import api from "../../services/axiosConfig";

const VerifyEmailPage = () => {
  const [searchParams] = useSearchParams();
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

  return (
    <div className="min-h-screen flex items-center justify-center">
      {status === "loading" && <p>Verifying your email...</p>}
      {status === "success" && <p className="text-green-600">Your email has been verified! You can now log in.</p>}
      {status === "error" && <p className="text-red-600">Verification failed. The link may be invalid or expired.</p>}
    </div>
  );
};

export default VerifyEmailPage;