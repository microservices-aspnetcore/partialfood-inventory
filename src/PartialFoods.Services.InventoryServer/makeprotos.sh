PROJDIR=`pwd`
cd ~/.nuget/packages/grpc.tools/1.6.1/tools/linux_x64
protoc -I $PROJDIR/../../proto --csharp_out $PROJDIR/RPC --grpc_out $PROJDIR/RPC $PROJDIR/../../proto/inventory.proto --plugin=protoc-gen-grpc=grpc_csharp_plugin
cd -