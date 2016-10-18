FROM microsoft/dotnet:latest
ENV name SecuritiesPopulater
COPY src/$name /root/$name
RUN cd /root/$name && dotnet restore && dotnet build && dotnet publish
RUN cp -rf /root/$name/bin/Debug/netcoreapp1.0/publish/* /root/
ENTRYPOINT dotnet /root/${name}.dll
CMD 100000
