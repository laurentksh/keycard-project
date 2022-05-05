import axios from "axios";
import { useEffect } from "react";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import '../Home.css';
import { PunchFilterDto, PunchViewModel } from "../models/PunchModels";
import Table from "./Table";

const Home = (): JSX.Element => {
  const navigate = useNavigate()
  const token = localStorage.getItem("token");

  if (!token) {
    navigate("/login");
  }

  const [punches, setPunches] = useState<PunchViewModel[]>([]);
  const [loading, setLoading] = useState(false);

  async function postPunch(): Promise<PunchViewModel> {
    setLoading(true);
    const response = await axios.post("api/v1/Punch", {}, {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });
    setLoading(false);
    return response.data;
  }

  async function getPunches() : Promise<PunchViewModel[]> {
    setLoading(true);
    const response = await axios.post("api/v1/Punch/history", {
    } as PunchFilterDto, {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });
    setLoading(false);
    console.log(response.data)
    return response.data;
  }

  async function doGenPhys(): Promise<void> {
    setLoading(true);
    const response = await axios.post("api/v1/Auth/device", { deviceName: "Phyiscal device" }, {
      headers: {
        "Authorization": `Bearer ${token}`
      }
    });
    setLoading(false);
    alert("Your new physical key token: " + response.data.token)
  }

  async function doPunch() {
    await postPunch();
    await getPunches();
  }

  useEffect( () => {
      getPunches()
      .then(res => setPunches(res))
      .catch(error => {
        localStorage.removeItem("token");
        navigate("/login");
        console.log('error', error)
      });
  }, [navigate, getPunches]);

  return (
      <>
        <div>
          <Table punches={punches} loading={loading} />
        </div>
        <button disabled={loading} onClick={() => doPunch()}>Add +</button>
        <button disabled={loading} onClick={() => doGenPhys()}>Generate Physical Token</button>
      </>
  );
}

export default Home;
