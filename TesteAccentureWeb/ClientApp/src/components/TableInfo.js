import React, { Component } from 'react';
import { Container } from 'reactstrap';

export class TableInfo extends Component {
    static displayName = TableInfo.name;


    showData = () => {
        const { rowsPerPage, currentPage, data } = this.props;
        const indexOfLastPage = currentPage * rowsPerPage;
        const indexOfFirstPage = indexOfLastPage - rowsPerPage;
        const currentPosts = data;

        try {
            return currentPosts.map((item, index) => {
                return (
                    <tr key={index}>
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
        await this.props.setCurrentPage(pageNumbers);

        this.props.getLogInfos();
    }

    showPagination = () => {
        const { currentPage, totalPages, paginationCount, firtsPage, lastPage } = this.props;
        var pageNumbers = [];

        var initIndex = firtsPage;

        if (totalPages > paginationCount) {
            if ((lastPage - currentPage) < 4) {

                initIndex = (((totalPages - lastPage) <= 4) ?
                    (totalPages + 1) - paginationCount :
                    initIndex += 4);
            }
            else if ((currentPage - firtsPage) < 4) {

                initIndex = ((firtsPage - currentPage) > 0 ? initIndex -= 4 : 1);
            }
        }

        var maxPageCount = (totalPages < paginationCount) ? totalPages : (paginationCount + (initIndex - 1));

        for (let i = initIndex; i <= maxPageCount; i++) {
            if (i <= maxPageCount || i == totalPages || Math.abs(currentPage - i) <= 1) {
                pageNumbers.push(i);
            }
        }

        if (totalPages > paginationCount) {

            if (lastPage - (initIndex - 1) < pageNumbers.length) {
                this.props.setFistLastPage({ firtsPage: initIndex, lastPage: pageNumbers[pageNumbers.length - 1] });

                //this.setState({ firtsPage: initIndex, LastPage: pageNumbers[pageNumbers.length - 1] });
            }

            if (lastPage - (initIndex - 1) > pageNumbers.length) {
                this.props.setFistLastPage({ firtsPage: pageNumbers[0], lastPage: pageNumbers[pageNumbers.length - 1] });

                //this.setState({ firtsPage: pageNumbers[0], LastPage: pageNumbers[pageNumbers.length - 1] });
            }
        }

        return (
            <nav>
                <ul style={{ 'display': 'flex', 'listStyle': 'none', 'borderRadius': '0.25rem', 'justifyContent': 'center' }}>

                    {pageNumbers.map(number => (
                        <li key={number} className={this.props.currentPage === number ? 'page-item active' : 'page-item'}>
                            <button onClick={async () => { await this.pagination(number); }} className="page-link"> {number} </button>
                        </li>
                    ))}
                </ul>
            </nav>
        )


    }

    render() {
        return (
            <div>
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
                        {this.props.currentPage} de {this.props.totalPages}<br />
                        Total de registros: {this.props.totalRows}
                    </div>
                </div>
            </div>

        );
    }
}
