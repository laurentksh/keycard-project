import { PunchSource, PunchViewModel } from "../models/PunchModels";

export interface TableProps {
  punches: PunchViewModel[];
  loading: boolean;
}

const Table = (props: TableProps): JSX.Element => {
  return (
    <div className="table m-auto" style={{ backgroundColor: props.loading ? 'gray' : '' }}>
      <p>Résumé</p>
      <div className="flex flex-col m-auto">
        <div className="overflow-x-auto sm:-mx-6 lg:-mx-8">
          <div className="py-4 inline-block min-w-full sm:px-6 lg:px-8">
            <div className="overflow-hidden">
              <table className="min-w-full text-center">
                <thead className="border-b bg-gray-800">
                  <tr>
                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      Id
                    </th>
                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      Date
                    </th>
                    <th scope="col" className="text-sm font-medium text-white px-6 py-4">
                      Source
                    </th>
                    
                  </tr>
                </thead>  
                <tbody>
                  {props.punches.map((punch: PunchViewModel) => (
                    <>
                      <tr className="bg-white border-b" key={punch.id}>
                        <td className="px-6 py-4 whitespace-nowrap text-sm font-medium text-gray-900">{punch.id}</td>
                        <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">{punch.creationDate.replace("T", " ").replace("Z", " ")}</td>
                        <td className="text-sm text-gray-900 font-light px-6 py-4 whitespace-nowrap">{PunchSource[punch.source]}</td>
                      </tr>
                    </>
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