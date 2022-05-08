import axios from "axios";
import { PunchSource, PunchViewModel } from "../models/PunchModels";

export interface TableProps {
  punches: PunchViewModel[]
  refresh: () => void
}

const Table = (props: TableProps): JSX.Element => {
  async function removePunch(punch: PunchViewModel): Promise<void> {
    await axios.delete(`api/v1/Punch/${punch.id}`)
    props.refresh()
  }

  function getLabelForSource(source: PunchSource): string {
    switch (source) {
      case PunchSource.physical:
        return "Physical key"
      case PunchSource.webPortal:
        return "Web Portal"
      case PunchSource.unknown:
        return "Unknown"
    }
  }

  //shitty trim date function that works well
  function trimDate(date: string): string {
    date = date.substring(0, date.indexOf("."))
    date = date.replace("T", " ").replace("Z", " ").replaceAll("-", ".")
    return date
  }

  if (!props.punches || props.punches.length === 0)
    return <></>
  
  return (
    <div className="table m-auto">
      <p className="text-lg">Timesheet</p>
      <div className="flex flex-col m-auto">
        <div className="overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="py-4 inline-block min-w-full sm:px-6 lg:px-8">
            <div className="overflow-hidden">
              <table className="min-w-full text-center">
                <thead className="border-b bg-gray-800">
                  <tr>
                    <th scope="col" className="text-sm font-medium text-white lg:px-6 py-4">
                      Id
                    </th>
                    <th scope="col" className="text-sm font-medium text-white lg:px-6 py-4">
                      Date
                    </th>
                    <th scope="col" className="text-sm font-medium text-white lg:px-6 py-4">
                      Source
                    </th>
                    <th scope="col" className="text-sm font-medium text-white lg:px-6 py-4">

                    </th>
                  </tr>
                </thead>  
                <tbody>
                  {props.punches.map((punch: PunchViewModel, index) => (
                    <tr className="bg-white border-b" key={`${punch.id}-rowid`}>
                      <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{index + 1}</td>
                      <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">{trimDate(punch.creationDate)}</td>
                      <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">{getLabelForSource(punch.source)}</td>
                      <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">
                        <button onClick={() => removePunch(punch)} className="focus:outline-none text-white bg-red-700 hover:bg-red-800 focus:ring-4 focus:ring-red-300 font-medium rounded-lg text-sm px-3 py-2.5 mr-2 mb-2">Remove</button>
                      </td>
                    </tr>
                    ))
                  }
                </tbody>
              </table>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
}

export default Table;