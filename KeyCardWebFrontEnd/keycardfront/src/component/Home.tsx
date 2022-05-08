import axios from "axios"
import { useEffect } from "react"
import { useState } from "react"
import { useNavigate } from "react-router-dom"
import '../Home.css'
import { PunchFilterDto, PunchViewModel } from "../models/PunchModels"
import Table from "./Table"

const Home = (): JSX.Element => {
  const navigate = useNavigate()
  const token = localStorage.getItem("token")

  if (!token) {
    navigate("/login")
  }

  const [punches, setPunches] = useState<PunchViewModel[]>([])
  const [loading, setLoading] = useState(false)

  async function getPunches() : Promise<PunchViewModel[]> {
    const response = await axios.post("api/v1/Punch/history", {} as PunchFilterDto)
    return response.data
  }

  async function doGeneratePhysicalKey(): Promise<void> {
    const response = await axios.post("api/v1/Auth/device", { deviceName: "Physical device" })
    alert("Your new physical key token: " + response.data.token)
  }

  async function doPunch() {
    setLoading(true)
    await axios.post("api/v1/Punch")
    setPunches(await getPunches());
    setLoading(false)
  }
  
  useEffect(() => {
      getPunches()
        .then(res => setPunches(res))
        .catch(error => {
          localStorage.removeItem("token")
          console.log('error', error)
          navigate('/login');
        });
  }, [navigate]);

  const GetIsEvenMessage = () => {
    let statusMsg = "unknown"
    let className = "m-auto p-4 text-sm text-black rounded-lg"

    if (punches === null || punches.length === 0) {
      statusMsg = "No punches yet"
      className += " bg-gray-100"
    } else {
      if (punches.length % 2 === 0) {
        statusMsg = "You are currently on a break."
        className += " bg-yellow-100"
      } else {
        statusMsg = "You are currently working."
        className += " bg-blue-100"
      }
    }

    return (
    <div className={className}>
      <p>{statusMsg}</p>
    </div>)
  }

  return (
      <>
        <div className="md:ml-40 md:mr-40 bg-white shadow-md rounded px-8 pt-6 pb-8 mb-4 flex flex-col">
          <Table punches={punches} refresh={() => getPunches().then((res) => setPunches(res))} />
          
          <GetIsEvenMessage />
          <div className="w-auto mx-auto mt-6">
            <button className="text-white bg-blue-700 hover:bg-blue-800 focus:ring-4 focus:ring-blue-300 font-medium rounded-lg text-sm px-5 py-2.5 mr-2 mb-2 focus:outline-none" disabled={loading} onClick={() => doPunch()}>Add +</button>
            <button className="py-2.5 px-5 mr-2 mb-2 text-sm font-medium focus:outline-none rounded-lg border focus:z-10 focus:ring-4 focus:ring-gray-300 bg-gray-800 text-white border-gray-600 hover:text-gray-400 hover:bg-gray-700" disabled={loading} onClick={() => doGeneratePhysicalKey()}>Generate Physical Token</button>
          </div>
          <svg style={{ display: loading ? 'block' : 'none'}} role="status" className="w-8 h-8 text-gray-200 animate-spin dark:text-gray-600 fill-blue-600 mx-auto" viewBox="0 0 100 101" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M100 50.5908C100 78.2051 77.6142 100.591 50 100.591C22.3858 100.591 0 78.2051 0 50.5908C0 22.9766 22.3858 0.59082 50 0.59082C77.6142 0.59082 100 22.9766 100 50.5908ZM9.08144 50.5908C9.08144 73.1895 27.4013 91.5094 50 91.5094C72.5987 91.5094 90.9186 73.1895 90.9186 50.5908C90.9186 27.9921 72.5987 9.67226 50 9.67226C27.4013 9.67226 9.08144 27.9921 9.08144 50.5908Z" fill="currentColor"/>
              <path d="M93.9676 39.0409C96.393 38.4038 97.8624 35.9116 97.0079 33.5539C95.2932 28.8227 92.871 24.3692 89.8167 20.348C85.8452 15.1192 80.8826 10.7238 75.2124 7.41289C69.5422 4.10194 63.2754 1.94025 56.7698 1.05124C51.7666 0.367541 46.6976 0.446843 41.7345 1.27873C39.2613 1.69328 37.813 4.19778 38.4501 6.62326C39.0873 9.04874 41.5694 10.4717 44.0505 10.1071C47.8511 9.54855 51.7191 9.52689 55.5402 10.0491C60.8642 10.7766 65.9928 12.5457 70.6331 15.2552C75.2735 17.9648 79.3347 21.5619 82.5849 25.841C84.9175 28.9121 86.7997 32.2913 88.1811 35.8758C89.083 38.2158 91.5421 39.6781 93.9676 39.0409Z" fill="currentFill"/>
          </svg>
        </div>
      </>
  );
}

export default Home;
