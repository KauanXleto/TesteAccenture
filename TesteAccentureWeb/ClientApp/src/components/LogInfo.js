import React, { Component } from 'react';
import Dropdown from 'react-dropdown';
import 'react-dropdown/style.css';

export class LogInfo extends Component {
    static displayName = LogInfo.name;

    constructor(props) {
        super(props);
        this.state = {
            data: [],
            logTypes: [],
            dropDownLogTypes:[],
            rowsPerPage: 5,
            currentPage: 1,
            totalPages: 0,
            totalRows: 0,
            paginationCount: 14,

            logIdentification: '',
            logTypeId: '',
            logIp: '',
            description: '',

            firtsPage: 1,
            LastPage: 14
        }
    }

    filterInfo = async () => {

        await this.setState({
            currentPage: 1,
            firtsPage: 1,
            LastPage: 14,
        });

        this.getLogInfos();
    }

    getLogInfos = async () => {

        //https://localhost:44373/Log/GetLogInfo?Page=2&RowsPerpage=50&RowsPerpage=50
        var url = "https://localhost:44373/Log/GetLogInfo?";

        var parans = "";

        if (this.state.currentPage)
            parans += `Page=${this.state.currentPage}`;

        if (this.state.rowsPerPage)
            parans += `${(parans != "" ? "&" : "")}RowsPerPage=${this.state.rowsPerPage}`;

        if (this.state.logIdentification)
            parans += `${(parans != "" ? "&" : "")}LogIdentification=${this.state.logIdentification}`;

        if (this.state.logTypeId)
            parans += `${(parans != "" ? "&" : "")}LogTypeId=${this.state.logTypeId}`;

        if (this.state.logIp)
            parans += `${(parans != "" ? "&" : "")}LogIp=${this.state.logIp}`;

        if (this.state.description)
            parans += `${(parans != "" ? "&" : "")}Description=${this.state.description}`;

        url += parans;

        const obj = {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            }
        }

        await fetch(`${url}`, obj)
            .then((response) => response.json())
            .then((responseJson) => {
                //console.warn(responseJson);

                responseJson.data.map(x => {
                    if (this.state.logTypes) {

                        var logType = this.state.logTypes.find(y => y.id == x.logTypeId);

                        if (logType)
                            x.logTypeName = logType.name;
                    }
                })

                this.setState({
                    totalPages: responseJson.totalPages,
                    totalRows: responseJson.totalRows,
                    data: responseJson.data
                })
            })
            .catch((error) => {
                console.warn(error);
            })
    }

    getLogTypes = async () => {

        var url = "https://localhost:44373/Log/GetLogTypes";

        const obj = {
            method: "GET",
            headers: {
                Accept: "application/json",
                "Content-Type": "application/json"
            }
        }

        var dropDownLogTypes = [];

        await fetch(`${url}`, obj)
            .then((response) => response.json())
            .then((responseJson) => {
                //console.warn(responseJson);

                dropDownLogTypes.push({ value: 0, label: 'Selecione logType' });

                responseJson.map(x => {
                    dropDownLogTypes.push({ value: x.id, label: x.name })
                });

                this.setState({
                    logTypes: responseJson,
                    dropDownLogTypes: dropDownLogTypes
                });
            })
            .catch((error) => {
                console.warn(error);
            })
    }

    showData = () => {
        const { rowsPerPage, currentPage, data } = this.state;
        const indexOfLastPage = currentPage * rowsPerPage;
        const indexOfFirstPage = indexOfLastPage - rowsPerPage;
        const currentPosts = data;

        try {
            return currentPosts.map((item, index) => {
                return (
                        <tr>
                            <td>{rowsPerPage * (currentPage - 1) + index + 1}</td>
                            <td>{item.logDate}</td>
                            <td>{item.logDescription}</td>
                            <td>{item.logIdentification}</td>
                            <td>{item.logIp}</td>
                            <td>{item.logTypeName}</td>
                        </tr>
                )
            })
        } catch (e) {
            alert(e.message)
        }
    }

    pagination = async (pageNumbers) => {
        await this.setState({ currentPage: pageNumbers });

        this.getLogInfos();
    }

    showPagination = () => {
        const { currentPage, totalPages, paginationCount, firtsPage, LastPage } = this.state;
        var pageNumbers = [];

        var initIndex = firtsPage;

        if (totalPages > paginationCount) {
            if ((LastPage - currentPage) < 4) {

                initIndex = (((totalPages - LastPage) <= 4) ?
                                (totalPages + 1) - paginationCount : 
                                initIndex += 4);
            }
            else if ((currentPage - firtsPage) < 4) {

                initIndex = ((firtsPage - currentPage) > 0 ? initIndex -= 4 : 1);


                //if ((totalPages - LastPage) <= 4) {
                //    initIndex = (totalPages + 1) - paginationCount;
                //}
                //else {
                //    initIndex += 4;
                //}
                //if ((firtsPage - currentPage) > 0) {
                //    initIndex -= 4;
                //}
                //else {
                //    initIndex = 1;
                //}
            }
        }

        var maxPageCount = (totalPages < paginationCount) ? totalPages : (paginationCount + (initIndex - 1));

        for (let i = initIndex; i <= maxPageCount; i++) {
            if (i <= maxPageCount || i == totalPages || Math.abs(currentPage - i) <= 1) {
                pageNumbers.push(i);
            }
        }

        if (totalPages > paginationCount) {

            if (LastPage - (initIndex - 1) < pageNumbers.length) {
                this.setState({ firtsPage: initIndex, LastPage: pageNumbers[pageNumbers.length - 1] });
            }

            if (LastPage - (initIndex - 1) > pageNumbers.length) {
                this.setState({ firtsPage: pageNumbers[0], LastPage: pageNumbers[pageNumbers.length - 1] });
            }
        }

        return (
            <nav>
                <ul style={{ 'display': 'flex', 'listStyle': 'none', 'borderRadius': '0.25rem', 'justifyContent': 'center' }}>

                    {pageNumbers.map(number => (
                        <li key={number} className={this.state.currentPage === number ? 'page-item active' : 'page-item'}>
                            <button onClick={async () => { await this.pagination(number); }} className="page-link"> {number} </button>
                        </li>
                    ))}
                </ul>
            </nav>
        )


    }

    async componentDidMount() {
        await this.getLogTypes()
        await this.getLogInfos()
    }

    _onSelect = (item) => {
        console.log('item _onSelect', item)

        this.setState({
            logTypeId: item.value
        });
    }

    render() {
        return (
            <div className="container"
                style={{
                    margin: 'auto',
                   'marginTop': '3%',
                    width: '100%',
                    'borderRadius': '10px',
                    padding: '10px',
                    height: '650px'
                }}>

                <div className="row" style={{'marginBottom':'10px'}}>
                    <div className="col-sm-12" style={{ textAlign: 'center' }}>
                        <h3><b>Registros de log</b></h3>
                    </div>
                </div>

                <div style={{'padding': '10px',
                             'boxShadow': 'rgb(219 219 219) 0px 0px 20px 0px',
                             'borderRadius': '10px',
                             'marginBottom': '10px'}}>

                    <div className="row form-group">
                        <div className="col-sm-6">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="LogDescription"
                                id="LogDescription"
                                onChange={(e) => this.setState({ description: e.target.value })}
                                value={this.state.description}>
                            </input>
                        </div>
                        <div className="col-sm-6">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="LogIdentification"
                                id="LogIdentification"
                                onChange={(e) => this.setState({ logIdentification: e.target.value })}
                                value={this.state.logIdentification}>
                            </input>
                        </div>
                    </div>

                    <div className="row form-group">

                        <div className="col-sm-6">
                            <input
                                type="text"
                                className="form-control"
                                placeholder="LogIp"
                                id="LogIp"
                                onChange={(e) => this.setState({ logIp: e.target.value })}
                                value={this.state.logIp}>
                            </input>
                        </div>
                        <div className="col-sm-4">
                            <Dropdown
                                options={this.state.dropDownLogTypes}
                                onChange={this._onSelect}
                                value={this.state.dropDownLogTypes[0]}
                                placeholder="Select an option" />
                        </div>
                        <div className="col-sm-2">
                            <button
                                onClick={this.filterInfo}
                                type="button"
                                className="btn btn-primary float-right">
                                Consultar
                                <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" className="bi bi-search" viewBox="0 0 16 16" style={{ 'verticalAlign': 'middle', 'marginLeft': '5px'}}>
                                    <path d="M11.742 10.344a6.5 6.5 0 1 0-1.397 1.398h-.001c.03.04.062.078.098.115l3.85 3.85a1 1 0 0 0 1.415-1.414l-3.85-3.85a1.007 1.007 0 0 0-.115-.1zM12 6.5a5.5 5.5 0 1 1-11 0 5.5 5.5 0 0 1 11 0z" />
                                </svg>
                            </button>
                        </div>


                    </div>
                </div>

                <div style={{ 'width': '100%' }}>

                    <table className="table align-items-center justifyContent-center mb-0">
                        <thead>
                            <tr>
                                <th>Nro.</th>
                                <th>logDate</th>
                                <th>logDescription</th>
                                <th>logIdentification</th>
                                <th>logIp</th>
                                <th>logType</th>
                            </tr>
                        </thead>

                        <tbody>
                            {this.showData()}
                        </tbody>
                    </table>

                </div>

                <div>
                    <div style={{ alignContent: 'center', 'borderTop': '50px', 'paddingLeft': 'revert !important' }}>
                        {this.showPagination()}
                    </div>

                    <div style={{ alignContent: 'left', color: '#a5a5a5' }}>
                        {this.state.currentPage} de {this.state.totalPages}<br/>
                        Total de registros: {this.state.totalRows}
                    </div>
                </div>

            </div>
        );
    }
}