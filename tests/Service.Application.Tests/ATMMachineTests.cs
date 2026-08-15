using Itmo.Dev.Platform.Persistence.Abstractions.Transactions;
using NSubstitute;
using Service.Application.Abstractions.Persistence;
using Service.Application.Abstractions.Persistence.Queries;
using Service.Application.Abstractions.Persistence.Repositories;
using Service.Application.Contracts.Account.Operations;
using Service.Application.Model.Accounts;
using Service.Application.Model.Sessions;
using Service.Application.Services;
using Xunit;
using IsolationLevel = System.Data.IsolationLevel;

namespace Lab1.Tests;

public class ATMMachineTests
{
    [Fact]
    public async Task ATMMachineTest_Should_Withdraw_When_EnoughMoneyInTheAccount()
    {
        // Arrange
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.QueryAsync(
            Arg.Any<SessionQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<ISession>([session]).ToAsyncEnumerable());

        accountRepository.QueryAsync(
            Arg.Any<AccountQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<Account>([account]).ToAsyncEnumerable());

        account.TryDeposit(1000m);

        IPersistenceContext context = Substitute.For<IPersistenceContext>();
        context.Accounts.Returns(accountRepository);
        context.Operations.Returns(operationRepository);
        context.Sessions.Returns(sessionRepository);

        IPersistenceTransactionProvider transactionProvider = Substitute.For<IPersistenceTransactionProvider>();

        IPersistenceTransaction transaction = Substitute.For<IPersistenceTransaction>();

        transactionProvider
            .BeginTransactionAsync(
            Arg.Any<IsolationLevel>(),
            Arg.Any<CancellationToken>())
            .Returns(transaction);

        var accountService = new AccountService(
            context,
            transactionProvider);

        // Act
        Withdraw.Response response = await accountService.WithdrawAsync(
            new Withdraw.Request(session.SessionId, 300m),
            CancellationToken.None);

        // Assert
        Assert.IsType<Withdraw.Response.Success>(response);
        Assert.Equal(700m, account.Balance);
    }

    [Fact]
    public async Task ATMMachineTest_Should_NoWithdraw_When_NotEnoughMoneyInTheAccount()
    {
        // Arrange
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.QueryAsync(
            Arg.Any<SessionQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<ISession>([session]).ToAsyncEnumerable());

        accountRepository.QueryAsync(
            Arg.Any<AccountQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<Account>([account]).ToAsyncEnumerable());

        account.TryDeposit(1000m);

        IPersistenceContext context = Substitute.For<IPersistenceContext>();
        context.Accounts.Returns(accountRepository);
        context.Operations.Returns(operationRepository);
        context.Sessions.Returns(sessionRepository);

        IPersistenceTransactionProvider transactionProvider = Substitute.For<IPersistenceTransactionProvider>();

        IPersistenceTransaction transaction = Substitute.For<IPersistenceTransaction>();

        transactionProvider
            .BeginTransactionAsync(
                Arg.Any<IsolationLevel>(),
                Arg.Any<CancellationToken>())
            .Returns(transaction);

        var accountService = new AccountService(
            context,
            transactionProvider);

        // Act
        Withdraw.Response response = await accountService
            .WithdrawAsync(
                new Withdraw.Request(session.SessionId, 3000m),
                CancellationToken.None);

        // Assert
        Assert.IsType<Withdraw.Response.Failure>(response);
        Assert.Equal(1000m, account.Balance);
    }

    [Fact]
    public async Task ATMMachineTest_Should_Deposit_When_DepositValidAmount()
    {
        // Arrange
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.QueryAsync(
            Arg.Any<SessionQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<ISession>([session]).ToAsyncEnumerable());

        accountRepository.QueryAsync(
            Arg.Any<AccountQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<Account>([account]).ToAsyncEnumerable());

        IPersistenceContext context = Substitute.For<IPersistenceContext>();
        context.Accounts.Returns(accountRepository);
        context.Operations.Returns(operationRepository);
        context.Sessions.Returns(sessionRepository);

        IPersistenceTransactionProvider transactionProvider = Substitute.For<IPersistenceTransactionProvider>();

        IPersistenceTransaction transaction = Substitute.For<IPersistenceTransaction>();

        transactionProvider
            .BeginTransactionAsync(
                Arg.Any<IsolationLevel>(),
                Arg.Any<CancellationToken>())
            .Returns(transaction);

        var accountService = new AccountService(
            context,
            transactionProvider);

        // Act
        Deposit.Response response = await accountService.DepositAsync(
            new Deposit.Request(session.SessionId, 1000m),
            CancellationToken.None);

        // Assert
        Assert.IsType<Deposit.Response.Success>(response);
        Assert.Equal(1000m, account.Balance);
    }

    [Fact]
    public async Task ATMMachineTest_Should_NotDeposit_When_DepositInvalidAmount()
    {
        // Arrange
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.QueryAsync(
            Arg.Any<SessionQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<ISession>([session]).ToAsyncEnumerable());

        accountRepository.QueryAsync(
            Arg.Any<AccountQuery>(),
            Arg.Any<CancellationToken>())
            .Returns(new List<Account>([account]).ToAsyncEnumerable());

        IPersistenceContext context = Substitute.For<IPersistenceContext>();
        context.Accounts.Returns(accountRepository);
        context.Operations.Returns(operationRepository);
        context.Sessions.Returns(sessionRepository);

        IPersistenceTransactionProvider transactionProvider = Substitute.For<IPersistenceTransactionProvider>();

        IPersistenceTransaction transaction = Substitute.For<IPersistenceTransaction>();

        transactionProvider
            .BeginTransactionAsync(
                Arg.Any<IsolationLevel>(),
                Arg.Any<CancellationToken>())
            .Returns(transaction);

        var accountService = new AccountService(
            context,
            transactionProvider);

        // Act
        Deposit.Response response = await accountService.DepositAsync(
            new Deposit.Request(session.SessionId, -300m),
            CancellationToken.None);

        // Assert
        Assert.IsType<Deposit.Response.Failure>(response);
        Assert.Equal(0m, account.Balance);
    }
}

