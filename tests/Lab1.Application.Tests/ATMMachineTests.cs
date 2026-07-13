using Lab1.Application.Abstractions.Persistence;
using Lab1.Application.Abstractions.Persistence.Queries;
using Lab1.Application.Abstractions.Persistence.Repositories;
using Lab1.Application.Contracts.Account.Operations;
using Lab1.Application.Models.Accounts;
using Lab1.Application.Models.Sessions;
using Lab1.Application.Services;
using Lab1.Infrastructure.Persistence;
using NSubstitute;
using Xunit;

namespace Lab1.Tests;

public class AtmMachineTests
{
    [Fact]
    public void ATMMachineTest_Should_Withdraw_When_EnoughMoneyInTheAccount()
    {
        // Act
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.Query(Arg.Any<SessionQuery>()).Returns([session]);
        accountRepository.Query(Arg.Any<AccountQuery>()).Returns([account]);

        account.TryDeposit(1000m);

        var context = new PersistenceContext(accountRepository, operationRepository, sessionRepository);

        var accountService = new AccountService(context);

        // Arrange
        Withdraw.Response response = accountService.Withdraw(new Withdraw.Request(session.SessionId, 300m));

        // Assert
        Assert.IsType<Withdraw.Response.Success>(response);
        Assert.Equal(700m, account.Balance);
    }

    [Fact]
    public void ATMMachineTest_Should_NoWithdraw_When_NotEnoughMoneyInTheAccount()
    {
        // Act
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.Query(Arg.Any<SessionQuery>()).Returns([session]);
        accountRepository.Query(Arg.Any<AccountQuery>()).Returns([account]);

        account.TryDeposit(1000m);

        var context = new PersistenceContext(accountRepository, operationRepository, sessionRepository);

        var accountService = new AccountService(context);

        // Arrange
        Withdraw.Response response = accountService.Withdraw(new Withdraw.Request(session.SessionId, 3000m));

        // Assert
        Assert.IsType<Withdraw.Response.Failure>(response);
        Assert.Equal(1000m, account.Balance);
    }

    [Fact]
    public void ATMMachineTest_Should_Deposit_When_DepositvalidAmount()
    {
        // Act
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.Query(Arg.Any<SessionQuery>()).Returns([session]);
        accountRepository.Query(Arg.Any<AccountQuery>()).Returns([account]);

        IPersistenceContext context = new PersistenceContext(accountRepository, operationRepository, sessionRepository);

        var accountService = new AccountService(context);

        // Arrange
        Deposit.Response response = accountService.Deposit(new Deposit.Request(session.SessionId, 1000m));

        // Assert
        Assert.IsType<Deposit.Response.Success>(response);
        Assert.Equal(1000m, account.Balance);
    }

    [Fact]
    public void ATMMachineTest_Should_NotDeposit_When_DepositInvalidAmount()
    {
        // Act
        IAccountRepository accountRepository = Substitute.For<IAccountRepository>();
        ISessionRepository sessionRepository = Substitute.For<ISessionRepository>();
        IOperationRepository operationRepository = Substitute.For<IOperationRepository>();
        var account = new Account(
            new AccountId(5),
            "1488",
            "239");

        var session = new UserSession(account.AccountId);

        sessionRepository.Query(Arg.Any<SessionQuery>()).Returns([session]);
        accountRepository.Query(Arg.Any<AccountQuery>()).Returns([account]);

        IPersistenceContext context = new PersistenceContext(accountRepository, operationRepository, sessionRepository);

        var accountService = new AccountService(context);

        // Arrange
        Deposit.Response response = accountService.Deposit(new Deposit.Request(session.SessionId, -300m));

        // Assert
        Assert.IsType<Deposit.Response.Failure>(response);
        Assert.Equal(0m, account.Balance);
    }
}